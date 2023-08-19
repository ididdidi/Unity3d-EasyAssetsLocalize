using System.Collections.Generic;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    /// <summary>
    /// The class contains the localization resources of the corresponding tag.
    /// </summary>
    [System.Serializable]
    public class Localization
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

        /// <summary>
        /// Whether the instance is standard for the type of resource.
        /// </summary>
        public bool IsDefault => isDefault;

        /// <summary>
        /// Constructor to create new instances with example resource object.
        /// Recommended for creating default localization
        /// </summary>
        /// <param name="name">Localization name</param>
        /// <param name="data">Resource data</param>
        /// <param name="numbLang">Number of languages</param>
        public Localization(string name, object data, int numbLang, bool isDefault = false)
        {
            if (data == null) { throw new System.ArgumentNullException(nameof(data)); }

            this.name = name;
            this.isDefault = isDefault;
            this.serializableType = new SerializableType(data.GetType());

            var resources = new IResource[numbLang];
            if (typeof(string).IsAssignableFrom(data.GetType()))
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resources[i] = new TextResource(data);
                }
            }
            else
            {
                for (int i = 0; i < resources.Length; i++)
                {
                    resources[i] = new UnityResource(data);
                }
            }
            this.resources = new List<IResource>(resources);
        }

        /// <summary>
        /// Constructor to create new instances with set of resource objects.
        /// </summary>
        /// <param name="type">Type of localized resources</param>
        /// <param name="resources">List of localized resources</param>
        public Localization(System.Type type, IEnumerable<IResource> resources)
        {
            this.name = $"{type.Name} Localization";
            this.serializableType = new SerializableType(type);
            this.resources = new List<IResource>(resources);
        }

        /// <summary>
        /// Recommended Method for Creating New Localization Instances.
        /// </summary>
        /// <returns>Copy of the localization instance</returns>
        public Localization Clone()
        {
            var resources = new IResource[Resources.Count];
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = Resources[i].Clone();
            }
            return new Localization(Type, resources);
        }
    }
}