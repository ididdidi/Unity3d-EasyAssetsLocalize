using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing references to localization objects.
    /// </summary>
    [System.Serializable]
    public abstract class LocalizationReceiver
    {
        [SerializeField] private string name;
        [SerializeField] private string id;
        [SerializeField] private Component[] components;
#if UNITY_EDITOR
        [System.NonSerialized] public bool open;
#endif
        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }
        public string Name { get => name; }
        public Component[] Components { get => components; set => components = value; }
        public abstract System.Type[] Types { get; }
        protected LocalizationReceiver(LocalizationTag localizationTag)
        {
            var resourceType = localizationTag.Resources[0]?.Data.GetType();
            this.name = $"{localizationTag.Name} ({resourceType.Name})";
            this.id = localizationTag.ID;
        }

        public void SetLocalizationResource(object resource)
        {
            for (int i = 0; i < components.Length; i++)
            {
                SetLocalization(components[i], resource);
            }
        }

        protected abstract void SetLocalization(Component receiver, object resource);
    }
}