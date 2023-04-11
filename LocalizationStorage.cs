using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField] private List<Localization> localizations = new List<Localization>();

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
            foreach(var local in Localizations)
            {
                if (local.Language.Equals(language))
                {
                    throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language}");
                }
            }
            
            var localization = new Localization(language);
            if(localizations.Count > 0)
            {
                foreach (var dictionary in localizations[localizations.Count - 1].Dictionary)
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