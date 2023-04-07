using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizationData : ScriptableObject
    {
        public List<LanguageData> Languages = new List<LanguageData>();

        public List<string> LanguageNames
        {
            get
            {
                List<string> languageNames = new List<string>();
                foreach (var language in Languages)
                {
                    languageNames.Add(language.Name);
                }
                return languageNames;
            }
        }

        public Dictionary<string, Resource> GetLanguageData(string languageName)
        {
            var dictionary = new Dictionary<string, Resource>();
            foreach (var language in Languages)
            {
                if (language.Name.Equals(languageName))
                {
                    foreach (var resource in language.Resources)
                    {
                        dictionary.Add(resource.Tag, resource);
                    }
                    return dictionary;
                }
            }
            return null;
        }
    }
}