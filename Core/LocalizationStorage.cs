using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Сlass that encapsulates data from languages and localized resources.
    /// </summary>
    public class LocalizationStorage : ScriptableObject
    {
        [SerializeField, HideInInspector] private int version; 
        [SerializeField, HideInInspector] private List<Language> languages = new List<Language>();
        [SerializeField, HideInInspector] private List<Localization> localizations = new List<Localization>();

        /// <summary>
        /// localization version. Depends on adding / removing languages and tags.
        /// </summary>
        public int Version { get => version; }
        /// <summary>
        /// List of languages in array format.
        /// </summary>
        public Language[] Languages => languages.ToArray();
        /// <summary>
        /// List of localizations in array format.
        /// </summary>
        public Localization[] Localizations => localizations.ToArray();

        /// <summary>
        /// Adds a new localization language to the repository.
        /// </summary>
        /// <param name="language">New localization language</param>
        public void AddLanguage(Language language)
        {
            if (languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language.Name}"); }

            for (int i = 0; i < localizations.Count; i++)
            {
             //   localizations[i].Resources.Add(localizations[i].Resources[0].Clone());
            }
            languages.Add(language);
            version++;
        }

        /// <summary>
        /// Removes the selected localization language from the repository.
        /// </summary>
        /// <param name="language">Localization language</param>
        public void RemoveLanguage(Language language)
        {
            if (!languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: No resources found for {language.Name}"); }

            var index = languages.IndexOf(language);
            for (int i=0; i < localizations.Count; i++)
            {
                localizations[i].Resources.RemoveAt(index);
            }
            languages.RemoveAt(index);
            version++;
        }

        /// <summary>
        /// Checks if the language is in the repository
        /// </summary>
        /// <param name="language">Localization language</param>
        /// <returns></returns>
        public bool ConteinsLanguage(Language language) => languages.Contains(language);

        /// <summary>
        /// Adds a new localization object to the repository.
        /// </summary>
        /// <param name="name">Object name</param>
        /// <param name="resource">Default localization resource</param>
        /// <returns>Identifier of the localization tag in the repository</returns>
        public string AddLocalization(string name, Object resource)
        {
            var resources = new Object[languages.Count];
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = resource;
            }
            var localization = new Localization(name, resources);
            localizations.Add(localization);
            version++;
            Debug.Log(localization.ID);
            return localization.ID;
        }

        /// <summary>
        /// Used to get localized resources by localization tag ID, which is assigned when adding localization to the repository.
        /// </summary>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        /// <returns>Localization</returns>
        public Localization GetLocalization(LocalizationTag tag)
        {
            for(int i=0; i < localizations.Count; i++)
            {
                if (localizations[i].ID.Equals(tag.ID))
                {
                    return localizations[i];
                } 
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {tag.name}");
        }

        /// <summary>
        /// Checks for localization by localization tag ID, which is assigned when adding localization to the repository.
        /// </summary>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        /// <returns><see>true</see> if the localization for the tag is in the repository, otherwise <see>false</see></returns>
        public bool ConteinsLocalization(LocalizationTag tag)
        {
            for (int i = 0; i < localizations.Count; i++)
            {
                if (localizations[i].ID.Equals(tag.ID)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Serves to insert localization at the specified index.
        /// </summary>
        /// <param name="index">Place on the list to insert</param>
        /// <param name="localization">Localization to insert</param>
        public void InsertLocalization(int index, Localization localization)
        {
            localizations.Insert(index, localization);
            version++;
        }

        /// <summary>
        /// Deletes localization data from storage.
        /// </summary>
        /// <param name="tag"><see cref="LocalizationTag"/></param>
        public void RemoveLocalization(LocalizationTag tag)
        {
            for (int i = localizations.Count-1; i > -1; i--)
            {
                if (localizations[i].ID.Equals(tag.ID)) { 
                    localizations.RemoveAt(i);
                    version++;
                }
            }
        }

        /// <summary>
        /// Removes localization data by index in the list of localizations.
        /// </summary>
        /// <param name="index">Index in the list of localizations</param>
        public void RemoveLocalization(int index)
        {
            localizations.RemoveAt(index);
            version++;
        }

        /// <summary>
        /// Method for getting a dictionary of localization resources for the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <returns><see>Dictionary</see> localization resources for the specified language</returns>
        public Dictionary<string, Object> GetDictionary(Language language)
        {
            if (!languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: No resources found for {language}"); }

            var index = languages.IndexOf(language);
            var dictionary = new Dictionary<string, Object>();
            for (int i= 0; i < localizations.Count; i++)
            {
                dictionary.Add(localizations[i].ID, localizations[i].Resources[index]);
            }
            return dictionary;
        }
    }
}