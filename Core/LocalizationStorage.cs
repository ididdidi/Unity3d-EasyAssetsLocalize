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
        [SerializeField, HideInInspector] private List<LocalizationTag> localizationTags = new List<LocalizationTag>();

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
        public LocalizationTag[] LocalizationTags => localizationTags.ToArray();

        /// <summary>
        /// Adds a new localization language to the repository.
        /// </summary>
        /// <param name="language">New localization language</param>
        public void AddLanguage(Language language)
        {
            if (languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: There is already a localizations with {language.Name}"); }

            for (int i = 0; i < localizationTags.Count; i++)
            {
                localizationTags[i].Resources.Add(localizationTags[i].Resources[0].Clone());
            }
            languages.Add(language);
            version++;
        }

        /// <summary>
        /// Removes the selected localization language from the repository.
        /// </summary>
        /// <param name="language">LocalizationTaglanguage</param>
        public void RemoveLanguage(Language language)
        {
            if (!languages.Contains(language)) { throw new System.ArgumentException($"{GetType()}: No resources found for {language.Name}"); }

            var index = languages.IndexOf(language);
            for (int i=0; i < localizationTags.Count; i++)
            {
                localizationTags[i].Resources.RemoveAt(index);
            }
            languages.RemoveAt(index);
            version++;
        }

        /// <summary>
        /// Checks if the language is in the repository
        /// </summary>
        /// <param name="language">LocalizationTaglanguage</param>
        /// <returns></returns>
        public bool ConteinsLanguage(Language language) => languages.Contains(language);

        /// <summary>
        /// Adds a new localization object to the repository.
        /// </summary>
        /// <param name="name">Object name</param>
        /// <param name="resource">Default localization resource</param>
        /// <returns>Identifier of the localization tag in the repository</returns>
        public void AddLocalizationTag(LocalizationTag tag)
        {
            for (int i = 1; i < languages.Count; i++)
            {
                tag.Resources.Add(tag.Resources[i - 1].Clone());
            }
            localizationTags.Add(tag);
            version++;
            return;
        }

        /// <summary>
        /// Used to get localized resources by localization tag ID, which is assigned when adding localization to the repository.
        /// </summary>
        /// <param name="id">Identifier of the localization tag in the repository</param>
        /// <returns>Localization</returns>
        public LocalizationTag GetLocalizationTag(string id)
        {
            for(int i=0; i < localizationTags.Count; i++)
            {
                if (localizationTags[i].ID.Equals(id))
                {
                    return localizationTags[i];
                } 
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {id}");
        }

        /// <summary>
        /// Serves to insert localization at the specified index.
        /// </summary>
        /// <param name="index">Place on the list to insert</param>
        /// <param name="localizationTag">LocalizationTag to insert</param>
        public void InsertLocalizationTag(int index, LocalizationTag localizationTag)
        {
            localizationTags.Insert(index, localizationTag);
            version++;
        }

        /// <summary>
        /// Removes localization data by index in the list of localizations.
        /// </summary>
        /// <param name="index">Index in the list of localizationTags</param>
        public void RemoveLocalizationTag(int index)
        {
            localizationTags.RemoveAt(index);
            version++;
        }

        public object GetLocalizationResource(string id, Language language)
        {
            for (int i = 0; i < localizationTags.Count; i++)
            {
                if (localizationTags[i].ID.Equals(id))
                {
                    return localizationTags[i].Resources[languages.IndexOf(language)].Data;
                }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {id}");
        }
    }
}