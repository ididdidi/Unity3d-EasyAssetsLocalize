using System.Collections.Generic;
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
        [SerializeField] private string name;
        [SerializeReference] private List<IResource> resources;

        /// <summary>
        /// Tag ID.
        /// </summary>
        public string ID { get => id; }
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// List of localized resources.
        /// </summary>
        public List<IResource> Resources { get => resources; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="resources">List of localized resources</param>
        public LocalizationTag(string name, IEnumerable<IResource> resources)
        {
            this.name = name;
            this.resources = new List<IResource>(resources);
        }
    }
}