using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
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
            get => new Language(PlayerPrefs.HasKey("Language") ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString());
            set => PlayerPrefs.SetString("Language", value.Name);
        }

        /// <summary>
        /// Link to localization repository.
        /// </summary>
        public static LocalizationStorage LocalizationStorage
        {
            get
            {
                try
                {
                    if (!localizationStorage) { localizationStorage = Resources.Load<LocalizationStorage>(nameof(ResourceLocalization.LocalizationStorage)); }
                }
                catch (System.Exception exp) { Debug.LogError(exp); }
                return localizationStorage;
            }
        }

        public static void AddLocalizationComponent(LocalizationComponent component)
        {
            if(!components.Contains(component)) { components.Add(component); }
            component.SetLocalizationData(LocalizationStorage.GetLocalizationData(component.ID, Language));
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
            var languages = new List<Language>(localizationStorage.Languages);
            if (languages.Count < 2) { return; }

            int index = languages.IndexOf(Language);
            if (index > -1)
            {
                int newIndex = (index + (int)direction) % languages.Count;
                if (newIndex >= 0) { index = newIndex; }
                else { index = languages.Count + newIndex; }
                SetLanguage(languages[index]);
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
                components[i].SetLocalizationData(LocalizationStorage.GetLocalizationData(components[i].ID, Language));
            }
        }
    }
}
