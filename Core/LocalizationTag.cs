using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    public abstract class LocalizationTag : MonoBehaviour
    {
        [SerializeField, HideInInspector] private string id;

        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }

        /// <summary>
        /// Default localization resource
        /// </summary>
        public abstract Resource Resource { get; set; }
    }
}
