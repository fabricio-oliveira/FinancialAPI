
using FinancialApi.Config;
using Microsoft.Extensions.Configuration;
using FinancialApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialApi.IntegrationTests.Confs
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<DataBaseContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase("InMemoryDb"), 10);

            services.AddMvc();
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseMvcWithDefaultRoute();
        }

    }
}