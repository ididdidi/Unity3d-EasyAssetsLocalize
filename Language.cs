using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class Language
    {
        [SerializeField] private string name;
        [SerializeReference] private List<Resource> resources;

        public Language(string name, IEnumerable<Resource> resources)
        {
            this.name = name;
            this.resources = new List<Resource>(resources);
        }

        public string Name { get => name; set => name = value; }

        public List<Resource> Resources { get => resources; }
    }
}