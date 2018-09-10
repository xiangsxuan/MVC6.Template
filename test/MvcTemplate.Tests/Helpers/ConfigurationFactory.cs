using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Tests
{
    public static class ConfigurationFactory
    {
        private static IConfiguration Config { get; }

        static ConfigurationFactory()
        {
            Dictionary<String, String> config = new Dictionary<String, String>();
            config.Add("Languages:Path", "languages.config");
            config.Add("SiteMap:Path", "mvc.sitemap");
            config.Add("Application:Path", "data");
            config.Add("Application:Env", "Test");

            Config = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }
        public static IConfiguration Create()
        {
            return Config;
        }
    }
}
