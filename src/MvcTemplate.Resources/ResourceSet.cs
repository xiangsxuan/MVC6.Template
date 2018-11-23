using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MvcTemplate.Resources
{
    internal class ResourceSet
    {
        private Dictionary<String, Dictionary<String, ResourceDictionary>> Source { get; }

        public ResourceSet()
        {
            Source = new Dictionary<String, Dictionary<String, ResourceDictionary>>();
        }

        public String this[String language, String group, String key]
        {
            get
            {
                if (!Source.ContainsKey(language))
                    return null;

                if (!Source[language].ContainsKey(group))
                    return null;

                return Source[language][group].TryGetValue(key, out String title) ? title : null;
            }
            set
            {
                if (!Source.ContainsKey(language))
                    Source[language] = new Dictionary<String, ResourceDictionary>();

                if (!Source[language].ContainsKey(group))
                    Source[language][group] = new ResourceDictionary();

                Source[language][group][key] = value;
            }
        }

        public void Add(String language, String source)
        {
            Source[language] = JsonConvert.DeserializeObject<Dictionary<String, ResourceDictionary>>(source);
        }
    }
}
