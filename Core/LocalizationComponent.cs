using UnityEngine;

namespace SimpleLocalization
{
    /// <summary>
    /// Provides a soft link between the scene object and resource localization.
    /// </summary>
    public abstract class LocalizationComponent : MonoBehaviour
    {
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
        public abstract void SetLocalizationData(object data);

        /// <summary>
        /// Subscribe to localization changes.
        /// </summary>
        private void Awake() => this.Subscribe();

        /// <summary>
        /// Unsubscribe to localization changes.
        /// </summary>
        private void OnDestroy() => this.Unsubscribe();
    }
}
