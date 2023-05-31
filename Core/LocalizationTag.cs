using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    [System.Serializable]
    public class LocalizationTag
    {
        [SerializeField, HideInInspector] private string id;
        [SerializeField] private Object[] receivers;

        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }


    }
}
