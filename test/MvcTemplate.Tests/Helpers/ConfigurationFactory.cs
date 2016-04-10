using Microsoft.Extensions.Configuration;

namespace MvcTemplate.Tests
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create()
        {
            return new ConfigurationBuilder().AddJsonFile("configuration.json").Build();
        }
    }
}
