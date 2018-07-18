using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialApi.Services;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using FinancialApi.workers;
using Microsoft.Extensions.Logging;
using Hangfire;

namespace FinancialApi.Config
{
    public class Startup
    {
        const int MAX_WORK_PRO_PROCESS = 5;
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

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionDatabase = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
            if (connectionDatabase == null)
                throw new ArgumentException("DATABASE_CONNECTION cannot be null");

            services.AddDbContextPool<DataBaseContext>(options => options.UseSqlServer(connectionDatabase), 20);
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
                throw new ArgumentException("QUEUE_CONNECTION cannot be null");

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
            services.AddTransient<IInterestRepository, InterestRepository>();
            services.AddTransient<IEntryRepository, EntryRepository>();

            //Service
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();
            services.AddSingleton<IBalanceService, BalanceService>();

        }


        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory,
                              IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //DotNetServer
            app.UseMvc();

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial API V1"));

            //Create database
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DataBaseContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            var options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * MAX_WORK_PRO_PROCESS };
            app.UseHangfireServer(options);
            app.UseHangfireDashboard("/hangfire");

            Jobs(serviceProvider);
        }

        void Jobs(IServiceProvider serviceProvider)
        {
            var consolidateEntryWorker = new ConsolidateEntryWorker(serviceProvider.GetService<IPaymentService>(),
                                                                    serviceProvider.GetService<IPaymentQueue>(),
                                                                    serviceProvider.GetService<IReceiptService>(),
                                                                    serviceProvider.GetService<IReceiptQueue>(),
                                                                    serviceProvider.GetService<IErrorQueue>(),
                                                                    serviceProvider.GetService<ILogger<ConsolidateEntryWorker>>());

            BackgroundJob.Enqueue(() => consolidateEntryWorker.WorkManagerPay());
            BackgroundJob.Enqueue(() => consolidateEntryWorker.WorkManagerReceipt());


            var updateBalanceWorker = new UpdateBalanceWorker(serviceProvider.GetService<IBalanceService>(),
                                                              serviceProvider.GetService<IAccountRepository>());

            RecurringJob.AddOrUpdate(() => updateBalanceWorker.WorkManagement(), Cron.MinuteInterval(1));
        }
    }
}
