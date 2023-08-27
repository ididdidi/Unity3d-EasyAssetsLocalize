using System.Collections.Generic;
using UnityEngine;

namespace EasyAssetsLocalize
{
    // Class for managing resource localization
    public static class LocalizationManager
    {
        private static IStorage localizationStorage;
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
        public static IStorage Storage
        {
            get
            {
                if (localizationStorage == null) { 
                    localizationStorage = Resources.Load<LocalizationStorage>(nameof(LocalizationStorage));
                }
                return localizationStorage;
            }
        }

        /// <summary>
        /// Method for getting component localization.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        /// <returns><see cref="Localization"/> for a given component</returns>
        public static Localization GetLocalization(this LocalizationComponent component)
        {
            try
            {
                return Storage.GetLocalization(component.ID);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message.Replace(component.ID, component.name));
                return Storage.GetDefaultLocalization(component.Type);
            }
        }

        /// <summary>
        /// Method for get localization resource data depending on the current language.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        /// <returns>Resource data</returns>
        public static object GetLocalizationData(LocalizationComponent component)
        {
            var localization = GetLocalization(component);
            return localization.Resources[Storage.Languages.IndexOf(Language)].Data;
        }

        /// <summary>
        /// Subscribe to localization changes.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        public static void Subscribe(this LocalizationComponent component)
        {
            if(!string.IsNullOrEmpty(component.ID) && !components.Contains(component)) { components.Add(component); }

            component.SetData(GetLocalizationData(component));
        }

        /// <summary>
        /// Unsubscribe to localization changes.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        public static void Unsubscribe(this LocalizationComponent component)
        {
            if (!string.IsNullOrEmpty(component.ID) && components.Contains(component)) { components.Remove(component); }
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
        /// <param name="language">Language</param>
        private static void SetLanguage(Language language)
        {
            Language = language;
            for (int i = 0; i < components.Count; i++)
            {
                components[i].SetData(GetLocalizationData(components[i]));
            }
        }
    }
}
