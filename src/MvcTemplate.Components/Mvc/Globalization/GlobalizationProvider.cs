using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class GlobalizationProvider : IGlobalizationProvider
    {
        public Language[] Languages
        {
            get;
        }
        public Language DefaultLanguage
        {
            get;
        }
        public Language CurrentLanguage
        {
            get
            {
                return Languages.Single(language => language.Culture.Equals(CultureInfo.CurrentUICulture));
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = value.Culture;
                Thread.CurrentThread.CurrentUICulture = value.Culture;
            }
        }
        private Dictionary<String, Language> LanguageDictionary
        {
            get;
        }

        public GlobalizationProvider(IConfiguration config)
        {
            String path = Path.Combine(config["Application:Path"], config["Globalization:Path"]);
            LanguageDictionary = new Dictionary<String, Language>();
            XElement languages = XElement.Load(path);

            foreach (XElement lang in languages.Elements("language"))
            {
                Language language = new Language();
                language.Culture = new CultureInfo((String)lang.Attribute("culture"));
                language.IsDefault = (Boolean?)lang.Attribute("default") == true;
                language.Abbreviation = (String)lang.Attribute("abbreviation");
                language.Name = (String)lang.Attribute("name");

                LanguageDictionary.Add(language.Abbreviation, language);
            }

            Languages = LanguageDictionary.Select(language => language.Value).ToArray();
            DefaultLanguage = Languages.Single(language => language.IsDefault);
        }

        public Language this[String abbreviation] => LanguageDictionary[abbreviation];
    }
}
