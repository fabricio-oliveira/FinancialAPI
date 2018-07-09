using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FinancialApi.Services;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi
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

            services.AddDbContext<DataBaseContext>();

            // Add framework services.
            services.AddMvc();
            services.AddSingleton<QueueContext>(new QueueContext(Environment.GetEnvironmentVariable("QUEUE_CONNECTION")));

            //Queue
            services.AddSingleton<PaymentQueue, PaymentQueue>();
            services.AddSingleton<ReceiptQueue, ReceiptQueue>();

            //Repository
            services.AddSingleton<AccountRepository, AccountRepository>();
            services.AddSingleton<CashFlowRepository, CashFlowRepository>();
            services.AddSingleton<ChargeRepository, ChargeRepository>();
            services.AddSingleton<InputRepository, InputRepository>();
            services.AddSingleton<OutputRepository, OutputRepository>();

            //Service
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
