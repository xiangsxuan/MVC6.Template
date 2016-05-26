using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MvcTemplate.Web
{
    public class Program
    {
        public static void Main()
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseKestrel()
                .Build()
                .Run();
        }
    }
}
