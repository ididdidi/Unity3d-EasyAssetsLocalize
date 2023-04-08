using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public class LocalizationStorage : ScriptableObject
    {
        public List<Localization> localizations = new List<Localization>();

        public List<string> Languages
        {
            get
            {
                List<string> languages = new List<string>();
                foreach (var localization in localizations)
                {
                    languages.Add(localization.Language);
                }
                return languages;
            }
        }

        public Dictionary<string, Resource> GetLocalizationResources(string language)
        {
            var dictionary = new Dictionary<string, Resource>();
            foreach (var localization in localizations)
            {
                if (localization.Language.Equals(language))
                {
                    foreach (var resource in localization.Resources)
                    {
                        dictionary.Add(resource.Tag, resource);
                    }
                    return dictionary;
                }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {language}");
        }
    }
}