using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NMAS.WebApi.Repositories.Bases;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMAS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            await webHost.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();
                    config.Sources.Clear();

                    var env = context.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();

                    LocalDatabase localDatabase = new LocalDatabase();
                    var databaseConnectionString = localDatabase.EnsureCreated();
                    config.AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", databaseConnectionString)
                    });

                })
            .UseStartup<Startup>();
    }
}
