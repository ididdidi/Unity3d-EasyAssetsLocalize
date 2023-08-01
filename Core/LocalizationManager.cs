using System.Collections.Generic;
using UnityEngine;

namespace SimpleLocalization
{
    public static class LocalizationManager
    {
        private static LocalizationStorage localizationStorage;
        private static List<LocalizationComponent> components = new List<LocalizationComponent>();

        /// <summary>
        /// The current language.
        /// </summary>
        public static Language Language
        {
            get => new Language(PlayerPrefs.HasKey("Language") ? (SystemLanguage)PlayerPrefs.GetInt("Language") : Application.systemLanguage);
            set => PlayerPrefs.SetInt("Language", (int)value.SystemLanguage);
        }

        public static Language[] Languages => Storage.Languages.ToArray();

        /// <summary>
        /// Link to localization repository.
        /// </summary>
        public static LocalizationStorage Storage
        {
            get
            {
                if (!localizationStorage) { 
                    localizationStorage = Resources.Load<LocalizationStorage>(nameof(LocalizationStorage));
                }
                return localizationStorage;
            }
        }

        public static void AddLocalizationComponent(LocalizationComponent component)
        {
            if(!components.Contains(component)) { components.Add(component); }
            component.SetLocalizationData(Storage.GetLocalizationData(component.ID, Language));
        }

        public static void RemoveLocalizationComponent(LocalizationComponent component)
        {
            if (components.Contains(component)) { components.Remove(component); }
        }

        /// <summary>
        /// Changes the language to the next one in the localization list.
        /// </summary>
        public static void SetNextLanguage()
        {
            ChangeLocalzation(Direction.Next);
        }

        /// <summary>
        /// Changes the language to the previous one in the list of localizations.
        /// </summary>
        public static void SetPrevLanguage()
        {
            ChangeLocalzation(Direction.Back);
        }

        private enum Direction { Next = 1, Back = -1 }
        /// <summary>
        /// Method for changing localization language. Switching is carried out in a circle.
        /// </summary>
        /// <param name="direction">Indicates which language to select: the previous one in the list or the next one</param>
        private static void ChangeLocalzation(Direction direction)
        {
            var languages = new List<Language>(Storage.Languages);
            if (languages.Count < 2) { return; }

            int index = languages.IndexOf(Language);
            if (index > -1)
            {
                int newIndex = (index + (int)direction) % languages.Count;
                if (newIndex >= 0) { index = newIndex; }
                else { index = languages.Count + newIndex; }
                SetLanguage(languages[index]);

                Debug.Log($"{languages[index]} has been selected");
            }
            else
            {
                throw new System.ArgumentException($"{Language} not found in the {localizationStorage}");
            }
        }

        /// <summary>
        /// Sets the current language and loads localized resources.
        /// </summary>
        /// <param name="language">LocalizationTaglanguage</param>
        private static void SetLanguage(Language language)
        {
            Language = language;
            for (int i = 0; i < components.Count; i++)
            {
                components[i].SetLocalizationData(Storage.GetLocalizationData(components[i].ID, Language));
            }
        }
    }
}
