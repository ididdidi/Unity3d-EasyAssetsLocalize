using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField, HideInInspector] private List<Language> languages = new List<Language>();
        [SerializeField, HideInInspector] private List<Localization> localizations = new List<Localization>();

        public Language[] Languages => languages.ToArray();

        public Localization[] Localizations => localizations.ToArray();

        public void AddLanguage(Language language)
        {
            if (languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language.Name}"); }

            for (int i = 0; i < localizations.Count; i++)
            {
                localizations[i].Resources.Add(localizations[i].Resources[0].Clone());
            }
            languages.Add(language);
        }

        public void RemoveLanguage(Language language)
        {
            if (!languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: No resources found for {language.Name}"); }

            var index = languages.IndexOf(language);
            for (int i=0; i < localizations.Count; i++)
            {
                localizations[i].Resources.RemoveAt(index);
            }
            languages.RemoveAt(index);
        }

        public bool ConteinsLocalization(string id)
        {
            for(int i=0; i < localizations.Count; i++)
            {
                if (localizations[i].ID.Equals(id)) { return true; }
            }
            return false;
        }

        public string AddLocalization(string name, Resource resource)
        {
            var resources = new Resource[languages.Count];
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = resource.Clone();
            }
            var localization = new Localization(name, resources);
            localizations.Add(localization);
            return localization.ID;
        }

        public Localization GetLocalization(string id)
        {
            for(int i=0; i < localizations.Count; i++)
            {
                if (localizations[i].ID.Equals(id))
                {
                    return localizations[i];
                } 
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {id}");
        }

        public void InsertLocalization(int index, Localization localization)
        {
            localizations.Insert(index, localization);
        }

        public void RemoveLocalization(string id)
        {
            for (int i = localizations.Count-1; i > -1; i--)
            {
                if (localizations[i].ID.Equals(id)) { localizations.RemoveAt(i); }
            }
        }

        public void RemoveLocalization(int index)
        {
            localizations.RemoveAt(index);
        }

        public Dictionary<string, Resource> GetDictionary(Language language)
        {
            if (!languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: No resources found for {language}"); }

            var index = languages.IndexOf(language);
            var dictionary = new Dictionary<string, Resource>();
            for (int i= 0; i < localizations.Count; i++)
            {
                dictionary.Add(localizations[i].ID, localizations[i].Resources[index]);
            }
            return dictionary;
        }
    }
}