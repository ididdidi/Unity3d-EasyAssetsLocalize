using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField] private List<Localization> localizations = new List<Localization>();

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

        public IEnumerable<Localization> Localizations { get => localizations; }

        public Localization GetLocalization(string language)
        {
            for (int i = localizations.Count - 1; i > -1; i--)
            {
                if (localizations[i].Language.Equals(language))
                {
                    return localizations[i];
                }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {language}");
        }

        public void AddLocalization(string language)
        {
            var count = localizations.Count;
            for (int i = 0; i < count; i++)
            {
                if (localizations[i].Language.Equals(language))
                {
                    throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language}");
                }
            }
            
            var localization = new Localization(language);
            if(count > 0)
            {
                foreach (var dictionary in localizations[count-1].Dictionary)
                {
                    localization.SetValue(dictionary.Key, dictionary.Value);
                }
            }
            localizations.Add(localization);
        }

        public void RemoveLocalization(string language)
        {
            for (int i = localizations.Count - 1; i > -1; i--)
            {
                if (localizations[i].Language.Equals(language))
                {
                    localizations.RemoveAt(i);
                    return; // In this implementation, tags should not be repeated, if this is allowed, then to remove all values ​​with a given tag, just comment out this return
                }
            }
        }
    }
}