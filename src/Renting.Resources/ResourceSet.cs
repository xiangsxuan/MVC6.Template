﻿using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Renting.Resources
{
    public class ResourceSet
    {
        private ConcurrentDictionary<String, ConcurrentDictionary<String, ResourceDictionary>> Source { get; }

        public ResourceSet()
        {
            Source = new ConcurrentDictionary<String, ConcurrentDictionary<String, ResourceDictionary>>();
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
                    Source[language] = new ConcurrentDictionary<String, ResourceDictionary>();

                if (!Source[language].ContainsKey(group))
                    Source[language][group] = new ResourceDictionary();

                Source[language][group][key] = value;
            }
        }

        public void Override(String language, String source)
        {
            Dictionary<String, ResourceDictionary> resources = JsonConvert.DeserializeObject<Dictionary<String, ResourceDictionary>>(source);

            foreach (String group in resources.Keys)
                foreach (String key in resources[group].Keys)
                    this[language, group, key] = resources[group][key];
        }

        public void Inherit(ResourceSet resources)
        {
            foreach (String language in resources.Source.Keys)
                foreach (String group in resources.Source[language].Keys)
                    foreach (String key in resources.Source[language][group].Keys)
                        if (this[language, group, key] == null)
                            this[language, group, key] = resources.Source[language][group][key];
        }
    }
}
