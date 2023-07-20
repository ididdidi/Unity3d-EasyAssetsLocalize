using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
        [SerializeField] private string type = "Sysem.string";
        [SerializeReference] private List<IResource> resources = new List<IResource>();

        /// <summary>
        /// Tag ID.
        /// </summary>
        public string ID { get => id; }
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get => name; set => name = value; }
        public System.Type Type {
            get {
                if(type.StartsWith("System.")) { return System.Type.GetType(type); }
                Assembly asm = typeof(Object).Assembly;
                return asm.GetType(type);
            }
        }
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
            this.name = $"New {type.Name}";
            this.type = type.ToString();
            this.resources = new List<IResource>(resources);
        }
    }
}