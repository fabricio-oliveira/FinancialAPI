using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialApi.IntegrationTests.Confs
{
    
        public class TestFixture<TStartup> : IDisposable
        {
            private readonly TestServer _server;

            public TestFixture()
                : this(Path.Combine("src"))
            {
            }

            protected TestFixture(string relativeTargetProjectParentDir)
            {
                var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
                var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

                var builder = new WebHostBuilder()
                    .UseContentRoot(contentRoot)
                    .ConfigureServices(InitializeServices)
                    .UseEnvironment("Development")
                    .UseStartup(typeof(TStartup));

                _server = new TestServer(builder);

                Client = _server.CreateClient();
                Client.BaseAddress = new Uri("http://localhost");
            }

            public HttpClient Client { get; }

            public void Dispose()
            {
                Client.Dispose();
                _server.Dispose();
            }

            protected virtual void InitializeServices(IServiceCollection services)
            {
                var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

                var manager = new ApplicationPartManager();
                manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));
                manager.FeatureProviders.Add(new ControllerFeatureProvider());
                //manager.FeatureProviders.Add(new ViewComponentFeatureProvider());

                services.AddSingleton(manager);
            }

            private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
            {
                // Get name of the target project which we want to test
                var projectName = startupAssembly.GetName().Name;

                // Get currently executing test project path
                var applicationBasePath = System.AppContext.BaseDirectory;

                // Find the path to the target project
                var directoryInfo = new DirectoryInfo(applicationBasePath);
                do
                {
                    directoryInfo = directoryInfo.Parent;

                    var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                    if (projectDirectoryInfo.Exists)
                    {
                        var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                        if (projectFileInfo.Exists)
                        {
                            return Path.Combine(projectDirectoryInfo.FullName, projectName);
                        }
                    }
                }
                while (directoryInfo.Parent != null);

                throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
            }
        }
    }