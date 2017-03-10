using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Data.Core;
using System.IO;

namespace MvcTemplate.Data
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, IHostingEnvironment env)
        {
            services.AddTransient<Context>();
            services.AddSingleton<DbContextOptions>(new DbContextOptionsBuilder<Context>().Options);
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(env.ContentRootPath).Parent.FullName)
                .AddJsonFile("MvcTemplate.Web/configuration.json").Build());
        }
    }
}
