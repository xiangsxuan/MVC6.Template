using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MvcTemplate.Data
{
    public class Program
    {
        public static void Main()
        {
        }

        public static IWebHost BuildWebHost(params String[] args)
        {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)
                    .AddJsonFile("MvcTemplate.Web/configuration.json")
                    .Build())
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();
        }
    }
}
