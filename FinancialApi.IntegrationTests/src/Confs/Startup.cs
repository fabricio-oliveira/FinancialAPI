using FinancialApi.Repositories;
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>(
                optionsBuilder => optionsBuilder.UseInMemoryDatabase("InMemoryDb"));
            
            services.AddMvc();
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IReceiptService, ReceiptService>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                var repository = app.ApplicationServices.GetService<PaymentRepository>();

                InitializeDatabase(repository);
            }

            app.UseMvcWithDefaultRoute();
        }

        public void InitializeDatabase(IPaymentRepository repo)
        {
            
        }
    }
}