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
        [SerializeField] private Behaviour[] components;
        [System.NonSerialized] public bool open;

        /// <summary>
        /// Identifier of the localization tag in the repository
        /// </summary>
        public string ID { get => id; set => id = value; }
        public string Name { get => name; }
        public Behaviour[] Components { get => components; set => components = value; }
        public abstract System.Type Type { get; }
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

        protected abstract void SetLocalization(Behaviour receiver, object resource);
    }
}