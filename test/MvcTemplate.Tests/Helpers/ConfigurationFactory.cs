using Microsoft.Extensions.Configuration;

namespace MvcTemplate.Tests
{
    public static class ConfigurationFactory
    {
        private static IConfiguration Config { get; set; }

        static ConfigurationFactory()
        {
            Config = new ConfigurationBuilder().AddJsonFile("configuration.json").Build();
        }
        public static IConfiguration Create()
        {
            return Config;
        }
    }
}
