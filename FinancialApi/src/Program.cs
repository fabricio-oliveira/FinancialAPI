using System.IO;
using FinancialApi.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace FinancialApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitWebServer();
        }

        private static void InitWebServer()
        {
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseIISIntegration()
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
