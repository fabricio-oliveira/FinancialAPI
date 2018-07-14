using System;
using System.Diagnostics;
using System.IO;
using FinancialApi.Config;
using FinancialApi.workers;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace FinancialApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitWebServer();
            EternalJob();

        }

        public static void EternalJob()
        {
            var jobFireForget = BackgroundJob.Enqueue<ConsolidateEntryWorker>(c => c.StartPayConsolidate());

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
