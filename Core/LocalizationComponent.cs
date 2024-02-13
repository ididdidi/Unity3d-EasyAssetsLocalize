using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Provides a soft link between the scene object and resource localization.
    /// </summary>
    public abstract class LocalizationComponent : MonoBehaviour
    {
        private IStorage Storage => LocalizationManager.Storage;
        /// <summary>
        /// Unique ID of the record in localization storage.
        /// </summary>
        [field: SerializeField, HideInInspector] public string ID { get; set; }

        /// <summary>
        /// Type of localization resources.
        /// </summary>
        public abstract System.Type Type { get; }

        /// <summary>
        /// Method for initializing objects in the scene when the application starts and the localization localization changes.
        /// </summary>
        /// <param name="data">Resource data for localization</param>
        protected abstract void SetData(object data);

        /// <summary>
        /// Method for get localization resource data depending on the current language.
        /// </summary>
        /// <param name="language">Current language</param>
        private void SetLanguage(Language language)
        {
            Localization localization;
            try
            {
                localization = Storage.GetLocalization(ID);
            }
            catch (System.ArgumentException exception)
            {
                localization = Storage.GetDefaultLocalization(Type);
                Debug.LogError($"{exception} for <color=aqua>{name}</color>", this);
            }
            SetData(localization?.Resources[Storage.Languages.IndexOf(language)].Data);
        }

        /// <summary>
        /// Subscribe to localization changes.
        /// </summary>
        private void OnEnable() => LocalizationManager.OnLanguageChange += SetLanguage;

        /// <summary>
        /// Unsubscribe to localization changes.
        /// </summary>
        private void OnDisable() => LocalizationManager.OnLanguageChange -= SetLanguage;
    }
}
