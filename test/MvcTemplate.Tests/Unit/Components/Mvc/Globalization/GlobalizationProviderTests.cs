using Microsoft.Extensions.Configuration;
using MvcTemplate.Components.Mvc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class GlobalizationProviderTests
    {
        private GlobalizationProvider provider;
        private static IConfiguration config;

        static GlobalizationProviderTests()
        {
            XElement english = new XElement("language");
            english.SetAttributeValue("default", "true");
            english.SetAttributeValue("name", "English");
            english.SetAttributeValue("culture", "en-GB");
            english.SetAttributeValue("abbreviation", "en");

            XElement lithuanian = new XElement("language");
            lithuanian.SetAttributeValue("name", "Lietuvių");
            lithuanian.SetAttributeValue("culture", "lt-LT");
            lithuanian.SetAttributeValue("abbreviation", "lt");

            XElement globalization = new XElement("globalization");
            globalization.Add(lithuanian);
            globalization.Add(english);

            config = ConfigurationFactory.Create();
            Directory.CreateDirectory(config["Application:Path"]);
            globalization.Save(Path.Combine(config["Application:Path"], config["Globalization:Path"]));
        }
        public GlobalizationProviderTests()
        {
            provider = new GlobalizationProvider(config);
        }

        #region CurrentLanguage

        [Fact]
        public void CurrentLanguage_ReturnsCurrentLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = provider["en"].Culture;

            Language actual = provider.CurrentLanguage;
            Language expected = provider["en"];

            Assert.Same(expected, actual);
        }

        [Fact]
        public void CurrentLanguage_SetsCurrentLanguage()
        {
            provider.CurrentLanguage = provider.Languages.Last();

            CultureInfo expectedCulture = provider.Languages.Last().Culture;
            CultureInfo actualUICulture = CultureInfo.CurrentUICulture;
            CultureInfo actualCulture = CultureInfo.CurrentCulture;

            Assert.Same(expectedCulture, actualUICulture);
            Assert.Same(expectedCulture, actualCulture);
        }

        #endregion

        #region GlobalizationProvider(String path)

        [Fact]
        public void GlobalizationProvider_LoadsAllLanguages()
        {
            Language ltLanguage = provider.Languages.First();
            Language enLanguage = provider.Languages.Last();

            Assert.Equal(new CultureInfo("en-GB"), enLanguage.Culture);
            Assert.Equal("en", enLanguage.Abbreviation);
            Assert.Equal("English", enLanguage.Name);
            Assert.True(enLanguage.IsDefault);

            Assert.Equal(new CultureInfo("lt-LT"), ltLanguage.Culture);
            Assert.Equal("lt", ltLanguage.Abbreviation);
            Assert.Equal("Lietuvių", ltLanguage.Name);
            Assert.False(ltLanguage.IsDefault);
        }

        [Fact]
        public void GlobalizationProvider_SetsDefaultLanguage()
        {
            Language actual = provider.DefaultLanguage;

            Assert.Equal(new CultureInfo("en-GB"), actual.Culture);
            Assert.Equal("en", actual.Abbreviation);
            Assert.Equal("English", actual.Name);
            Assert.True(actual.IsDefault);
        }

        #endregion

        #region this[String abbreviation]

        [Fact]
        public void Indexer_ReturnsLanguage()
        {
            Language actual = provider["en"];

            Assert.Equal(new CultureInfo("en-GB"), actual.Culture);
            Assert.Equal("en", actual.Abbreviation);
            Assert.Equal("English", actual.Name);
            Assert.True(actual.IsDefault);
        }

        #endregion
    }
}
