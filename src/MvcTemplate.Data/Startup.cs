using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcTemplate.Data.Core;

namespace MvcTemplate.Data
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Context>();
            services.AddInstance<DbContextOptions>(new DbContextOptionsBuilder<Context>().Options);
            services.AddInstance<IConfiguration>(new ConfigurationBuilder().AddJsonFile("../MvcTemplate.Web/configuration.json").Build());
        }
    }
}
