using System.Collections.Generic;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    /// <summary>
    /// The class contains the localization resources of the corresponding tag.
    /// </summary>
    [System.Serializable]
    public class LocalizationTag
    {
        [SerializeField] private string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private string name = "None";
        [SerializeField] private SerializableType serializableType;
        [SerializeReference] private List<IResource> resources = new List<IResource>();
        [SerializeField] private bool isDefault;

        /// <summary>
        /// Tag ID.
        /// </summary>
        public string ID { get => id; }
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get => name; set => name = value; }
        public System.Type Type => serializableType.type;
        /// <summary>
        /// List of localized resources.
        /// </summary>
        public List<IResource> Resources { get => resources; }
        public bool IsDefault => isDefault;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="resources">List of localized resources</param>
        public LocalizationTag(System.Type type, IEnumerable<IResource> resources)
        {
            this.name = $"{type.Name} Localization";
            this.serializableType = new SerializableType(type);
            this.resources = new List<IResource>(resources);
        }

        public LocalizationTag(string name, object value, int languages)
        {
            if (value == null) { throw new System.ArgumentNullException(nameof(value)); }

            this.name = name;
            this.isDefault = true;
            this.serializableType = new SerializableType(value.GetType());

            var resources = new IResource[languages];
            if (typeof(string).IsAssignableFrom(value.GetType()))
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resources[i] = new TextResource(value);
                }
            }
            else
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resources[i] = new UnityResource(value);
                }
            }
            this.resources = new List<IResource>(resources);
        }

        public LocalizationTag Clone()
        {
            var resources = new IResource[Resources.Count];
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = Resources[i].Clone();
            }
            return new LocalizationTag(Type, resources);
        }
    }
}