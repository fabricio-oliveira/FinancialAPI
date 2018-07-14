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

            services.AddSingleton<QueueContext>(
                new QueueContext(connectionQueue)
            );

            //Queue
            services.AddSingleton<PaymentQueue>();
            services.AddSingleton<ReceiptQueue>();

            //Repository
            services.AddTransient<AccountRepository>();
            services.AddTransient<BalanceRepository>();
            services.AddTransient<ChargeRepository>();
            services.AddTransient<EntryRepository>();

            //Service
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial API V1");
            });
        }
    }
}
