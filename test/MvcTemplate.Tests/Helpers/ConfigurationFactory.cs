using Microsoft.Extensions.Configuration;
using System.IO;

namespace MvcTemplate.Tests
{
    public static class ConfigurationFactory
    {
        private static IConfiguration Config { get; }

        static ConfigurationFactory()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("test-configuration.json")
                .Build();
        }
        public static IConfiguration Create()
        {
            return Config;
        }
    }
}
