using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LanguageData
    {
        [SerializeField] private string name;
        [SerializeField] private List<Resource> resources;

        public string Name { get => name; set => name = value; }

        public List<Resource> Resources { get => resources; }

        public LanguageData(string name)
        {
            this.name = name;
            resources = new List<Resource>();
        }
    }
}