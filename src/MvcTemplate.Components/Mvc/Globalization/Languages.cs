using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class Languages : ILanguages
    {
        public Language Default
        {
            get;
        }
        public Language Current
        {
            get
            {
                return Supported.Single(language => language.Culture.Equals(CultureInfo.CurrentUICulture));
            }
            set
            {
                Thread.CurrentThread.CurrentCulture = value.Culture;
                Thread.CurrentThread.CurrentUICulture = value.Culture;
            }
        }
        public Language[] Supported
        {
            get;
        }

        private Dictionary<String, Language> Dictionary
        {
            get;
        }

        public Languages(String config)
        {
            Dictionary = new Dictionary<String, Language>();

            foreach (XElement lang in XElement.Parse(config).Elements("language"))
            {
                Language language = new Language();
                language.Culture = new CultureInfo((String)lang.Attribute("culture"));
                language.Abbreviation = (String)lang.Attribute("abbreviation");
                language.Name = (String)lang.Attribute("name");

                if ((Boolean?)lang.Attribute("default") == true)
                    Default = language;

                Dictionary.Add(language.Abbreviation, language);
            }

            Supported = Dictionary.Values.ToArray();
        }

        public Language this[String abbreviation] => Dictionary.TryGetValue(abbreviation ?? "", out Language language) ? language : Default;
    }
}
