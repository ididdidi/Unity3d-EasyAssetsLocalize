using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class Localization
    {
        [SerializeField] private string language;
        [SerializeField] private List<Resource> resources;

        public string Language { get => language; set => language = value; }

        public List<Resource> Resources { get => resources; }

        public Localization(string language)
        {
            this.language = language;
            resources = new List<Resource>();
        }
    }
}