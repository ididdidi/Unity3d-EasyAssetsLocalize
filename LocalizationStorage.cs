using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField, HideInInspector] private List<LocalizationTag> tags = new List<LocalizationTag>();
        [SerializeField, HideInInspector] private List<Language> languages = new List<Language>();

        public Language[] Languages => languages.ToArray();

        public Localization[] Localizations
        {
            get
            {
                var localizations = new Localization[tags.Count];
                for(int i=0; i < tags.Count; i++)
                {
                    var resources = new Resource[languages.Count];
                    for(int j=0; j < languages.Count; j++)
                    {
                        resources[j] = languages[j].Resources[i];
                    }
                    localizations[i] = new Localization(tags[i], resources);
                }
                return localizations;
            }
        }

        public void AddLanguage(string name)
        {
            foreach (var language in languages)
            {
                if (language.Name.Equals(name))
                {
                    throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language}");
                }
            }

            var resources = new Resource[(languages.Count > 0) ? languages[0].Resources.Count : 0];
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = languages[0].Resources[i].Clone();
            }
            languages.Add(new Language(name, resources));
        }

        public void RemoveLanguage(string name)
        {
            for (int i = languages.Count - 1; i > -1; i--)
            {
                if (languages[i].Name.Equals(name))
                {
                    languages.RemoveAt(i);
                    return; // In this implementation, names should not be repeated, if this is allowed, then to remove all values ​​with a given name, just comment out this return
                }
            }
        }

        public bool Conteins(LocalizationTag tag)
        {
            return tags.Contains(tag);
        }

        public void AddResource(LocalizationTag tag, Resource resource)
        {
            if(tags.Contains(tag)) { throw new System.ArgumentException($"{tag.Name}-{tag.ID}: has already been added"); }

            tags.Add(tag);

            for(int i=0; i < languages.Count; i++) {
                languages[i].Resources.Add(resource);
            }
        }

        public void InsertResource(int index, Localization localization)
        {
            tags.Insert(index, localization.Tag);
            for (int i = 0; i < languages.Count; i++)
            {
                languages[i].Resources.Insert(index, localization.Resources[i]);
            }
        }

        public void RemoveResource(int index)
        {
            tags.RemoveAt(index);
            for (int i = 0; i < languages.Count; i++)
            {
                languages[i].Resources.RemoveAt(index);
            }
        }

        public void RemoveResource(LocalizationTag tag) => RemoveResource(tags.IndexOf(tag));

        public Dictionary<LocalizationTag, Resource> GetLocalization(string language)
        {
            for (int i = 0; i < languages.Count; i++)
            {
                if (languages[i].Name.Equals(language))
                {
                    var dictionary = new Dictionary<LocalizationTag, Resource>();
                    for (int j = 0; j < tags.Count; j++)
                    {
                        dictionary.Add(tags[j], languages[i].Resources[j]);
                    }
                    return dictionary;
                }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {language}");
        }

        public Dictionary<string, Resource> GetResources(LocalizationTag tag)
        {
            var resources = new Dictionary<string, Resource>();

            var index = tags.IndexOf(tag);
            for(int i=0; i < languages.Count; i++)
            {
                resources.Add(languages[i].Name, languages[i].Resources[index]);
            }

            return resources;
        }
    }
}