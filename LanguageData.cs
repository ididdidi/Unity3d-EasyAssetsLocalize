using System.Collections.Generic;
using UnityEngine;

namespace ru.mofrison.Unity3D.Localization
{
    [System.Serializable]
    public class LanguageData
    {
        [SerializeField] private string name;
        [SerializeField] private List<LocalizationResource> resources;

        public string Name { get => name; set => name = value; }

        public List<LocalizationResource> Resources { get => resources; }

        public LanguageData(string name)
        {
            this.name = name;
            resources = new List<LocalizationResource>();
        }
    }
}
