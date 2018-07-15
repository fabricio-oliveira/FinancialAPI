using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FinancialApi.Services;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using FinancialApi.workers;
using Hangfire;

namespace FinancialApi.Config
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionDatabase = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
            if (connectionDatabase == null)
                throw new System.ArgumentException("DATABASE_CONNECTION cannot be null");

            services.AddDbContextPool<DataBaseContext>(options => options.UseSqlServer(connectionDatabase), 10);
            services.AddHangfire(x => x.UseSqlServerStorage(connectionDatabase));


            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.DateFormatString = "dd-MM-yyyy";
                    });

            services.AddSwaggerGen(options =>
           {
               options.SwaggerDoc("v1",
                            new Info
                            {
                                Title = "Api de Finanças",
                                Version = "v1",
                                Description = "API REST de lançamentos Financeiros",
                                Contact = new Contact
                                {
                                    Name = "Fabricio Oliveira",
                                    Url = "https://github.com/fabricio.oliveira"
                                }
                            });
           });


            var connectionQueue = Environment.GetEnvironmentVariable("QUEUE_CONNECTION");
            if (connectionQueue == null)
                throw new System.ArgumentException("QUEUE_CONNECTION cannot be null");

            services.AddSingleton(
                new QueueContext(connectionQueue)
            );

            //Queue
            services.AddSingleton<IPaymentQueue, PaymentQueue>();
            services.AddSingleton<IReceiptQueue, ReceiptQueue>();
            services.AddSingleton<IErrorQueue, ErrorQueue>();

            //Repository
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IBalanceRepository, BalanceRepository>();
            services.AddTransient<ChargeRepository>();
            services.AddTransient<IEntryRepository, EntryRepository>();

            //Service
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();

            //Workers
            services.AddSingleton<ConsolidateEntryWorker>();
        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory,
                              IServiceProvider serviceProvider)
        {
            //Log
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            //DotNetServer
            app.UseMvc();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial API V1");
            });

            //HangFire Control job
            GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(serviceProvider));
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");


            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DataBaseContext>();
                dbContext.Database.EnsureCreated();
            }

            EternalJob(serviceProvider);
        }

        void EternalJob(IServiceProvider serviceProvider)
        {
            var consolidateEntry = new ConsolidateEntryWorker(serviceProvider.GetService<PaymentService>(),
                                                              serviceProvider.GetService<PaymentQueue>(),
                                                              serviceProvider.GetService<ReceiptService>(),
                                                              serviceProvider.GetService<ReceiptQueue>());

        }
    }
}
