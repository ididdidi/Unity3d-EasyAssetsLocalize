using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class Localization
    {
        [SerializeField] private string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private string name;
        [SerializeReference] private List<Resource> resources;

        public string ID { get => id; }
        public string Name { get => name; set => name = value; }

        public Localization(string name, IEnumerable<Resource> resources)
        {
            this.name = name;
            this.resources = new List<Resource>(resources);
        }

        public List<Resource> Resources { get => resources; }
    }
}