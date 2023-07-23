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
    }
}