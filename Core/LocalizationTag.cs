using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    [System.Serializable]
    public class LocalizationTag
    {
        [SerializeField] private string name;
        [SerializeField] private string id;
        [SerializeField] private string type;
        [SerializeField] private Object[] receivers;
        public bool open;
        
        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }
        public string Name { get => name; }
        public string Type { get => type; private set => type = value; }
        public Object[] Receivers { get => receivers; set => receivers = value; }

        public LocalizationTag(Localization localization)
        {
            var resourceType = localization.Resources[0]?.GetType();
            name = $"{localization.Name} ({resourceType.Name})";
            id = localization.ID;
            Type = resourceType.ToString();
        }
    }
}
