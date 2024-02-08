using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EasyAssetsLocalize
{
    [AddComponentMenu("Localize/Localization Controller")]
    public class LocalizationController : MonoBehaviour
    {
        private static LocalizationController instance;
        private List<LocalizationComponent> components = new List<LocalizationComponent>();

        [SerializeField] private LocalizationStorage localizationStorage;
        [System.Serializable] public class Handler : UnityEvent<string> { }
        [SerializeField] private Handler OnChangingLanguage;

        /// <summary>
        /// Link to Localization Storage
        /// </summary>
        public IStorage Storage { get => localizationStorage ?? (localizationStorage = Resources.Load<LocalizationStorage>(nameof(LocalizationStorage))); }

        /// <summary>
        /// The current language.
        /// </summary>
        public static Language Language
        {
            get => new Language(PlayerPrefs.HasKey("Language") ? (SystemLanguage)PlayerPrefs.GetInt("Language") : Application.systemLanguage);
            set => PlayerPrefs.SetInt("Language", (int)value.SystemLanguage);
        }

        /// <summary>
        /// Returns the available language
        /// </summary>
        private Language AvailableLanguage => Storage.Languages.Contains(Language) ? Language : Storage.Languages[0];

        /// <summary>
        /// Creates or returns a finished instance from the scene.
        /// </summary>
        /// <param name="dontDestroy">Whether to delete an object when moving to a new scene</param>
        /// <returns>Instance <see cref="LocalizationController"/></returns>
        public static LocalizationController GetInstance(bool dontDestroy = false)
        {
            if (!instance) { instance = FindObjectOfType<LocalizationController>(); }
            if (!instance)
            {
                var @object = new GameObject($"{nameof(LocalizationController)}");
                instance = @object.AddComponent<LocalizationController>();

                if (dontDestroy) { DontDestroyOnLoad(@object); }
            }
            return instance;
        }
        // Set language value at start
        private void Start() => OnChangingLanguage?.Invoke(AvailableLanguage.ToString());

        /// <summary>
        /// Method for get localization resource data depending on the current language.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        /// <returns>Resource data</returns>
        private object GetLocalizationData(LocalizationComponent component)
        {
            Localization localization;
            try
            {
                localization = Storage.GetLocalization(component);
            }
            catch (System.ArgumentException exception)
            {
                localization = Storage.GetDefaultLocalization(component.Type);
                Debug.LogError(exception, component);
            }
            return localization?.Resources[Storage.Languages.IndexOf(AvailableLanguage)].Data;
        }

        /// <summary>
        /// Subscribe to localization changes.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        public void Subscribe(LocalizationComponent component)
        {
            components.Add(component);
            component.SetData(GetLocalizationData(component));
        }

        /// <summary>
        /// Unsubscribe to localization changes.
        /// </summary>
        /// <param name="component"><see cref="LocalizationComponent"/></param>
        public void Unsubscribe(LocalizationComponent component) => components.Remove(component);

        /// <summary>
        /// Changes the language to the next one in the localization list.
        /// </summary>
        public void SetNextLanguage()
        {
            ChangeLocalzation(Direction.Next);
        }

        /// <summary>
        /// Changes the language to the previous one in the list of localizations.
        /// </summary>
        public void SetPrevLanguage()
        {
            ChangeLocalzation(Direction.Back);
        }

        /// <summary>
        /// Sets the current language and loads localized resources.
        /// </summary>
        /// <param name="language">Language</param>
        public void SetLanguage(Language language)
        {
            Language = language;
            OnChangingLanguage?.Invoke(language.ToString());
            for (int i = 0; i < components.Count; i++)
            {
                components[i].SetData(GetLocalizationData(components[i]));
            }
        }

        private enum Direction { Next = 1, Back = -1 }
        /// <summary>
        /// Method for changing localization language. Switching is carried out in a circle.
        /// </summary>
        /// <param name="direction">Indicates which language to select: the previous one in the list or the next one</param>
        private void ChangeLocalzation(Direction direction)
        {
            var languages = new List<Language>(Storage.Languages);
            if (languages.Count < 2) { return; }

            int index = languages.IndexOf(AvailableLanguage);
            int newIndex = (index + (int)direction) % languages.Count;
            if (newIndex >= 0) { index = newIndex; }
            else { index = languages.Count + newIndex; }
            SetLanguage(languages[index]);
        }
    }
}