using System.Collections.Generic;
using UnityEngine;

namespace EasyAssetsLocalize
{
    public static class LocalizationManager
    {
        private static IStorage storage;
        private static List<LocalizationComponent> components = new List<LocalizationComponent>();

        /// <summary>
        /// Link to Localization Storage
        /// </summary>
        public static IStorage Storage { get => storage ?? ResetStorage(); private set => storage = value; }

        /// <summary>
        /// The current language.
        /// </summary>
        public static Language Language
        {
            get => GetLanguage();
            set => SetLanguage(value);
        }
        
        /// <summary>
        /// Delegates subscribed to changes in the localizationtion storage.
        /// </summary>
        public static System.Action<IStorage> OnStorageChange { get; set; }
        public static System.Action<Language> OnLanguageChange { get; set; }

        public static void SetStorage(IStorage storage)
        {
            Storage = storage;
            OnStorageChange?.Invoke(Storage);
            OnLanguageChange?.Invoke(Language);
        }

        public static IStorage ResetStorage()
        {
            SetStorage(Resources.Load<LocalizationStorage>(nameof(LocalizationStorage)));
            return Storage;
        }

        /// <summary>
        /// Sets the current language and loads localized resources.
        /// </summary>
        /// <param name="language">Language</param>
        private static void SetLanguage(Language language)
        {
            if (storage.Languages.Contains(language))
            {
                PlayerPrefs.SetInt("Language", (int)language.SystemLanguage);
                OnLanguageChange?.Invoke(language);
            }
        }

        private static Language GetLanguage()
        {
            Language language = new Language(PlayerPrefs.HasKey("Language") ? 
                (SystemLanguage)PlayerPrefs.GetInt("Language") : Application.systemLanguage);

            return Storage.Languages.Contains(language) ? language : Storage.Languages[0];
        }
    }
}