using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Renting.Data
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
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();
        }
    }
}
