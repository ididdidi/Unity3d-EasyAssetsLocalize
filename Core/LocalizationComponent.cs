using UnityEngine;

namespace EasyAssetsLocalize
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
        /// Link to localization controller on stage
        /// </summary>
        public LocalizationController Controller { get; set; }

        /// <summary>
        /// Type of localization resources.
        /// </summary>
        public abstract System.Type Type { get; }

        /// <summary>
        /// Method for initializing objects in the scene when the application starts and the localization localization changes.
        /// </summary>
        /// <param name="data">Resource data for localization</param>
        public abstract void SetData(object data);

        private void OnValidate() => Controller = LocalizationController.GetInstance();

        /// <summary>
        /// Subscribe to localization changes.
        /// </summary>
        private void OnEnable() => Controller?.Subscribe(this);

        /// <summary>
        /// Unsubscribe to localization changes.
        /// </summary>
        private void OnDisable() => Controller?.Unsubscribe(this);
    }
}
