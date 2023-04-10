using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public partial class Localization
    {
        [SerializeField] private string language;
        [SerializeField] private List<SerializedResources> resources;

        public string Language { get => language; set => language = value; }

        public Localization(string language)
        {
            this.language = language;
            resources = new List<SerializedResources>();
        }
               
        public Dictionary<string, object> Dictionary
        {
            get
            {
                var dictionary = new Dictionary<string, object>();
                for (int i = 0; i < resources.Count; i++)
                {
                    dictionary.Add(resources[i].Tag, resources[i].Data);
                }
                return dictionary;
            }
        }

        public void SetValue(string tag, object value)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].Tag.Equals(tag)) { resources[i].Data = value; return; }
            }
            resources.Add(new SerializedResources(tag, value));
        }

        public object GetValue(string tag)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].Tag.Equals(tag)) { return resources[i].Data; }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {tag}");
        }

        public void Remove(string tag)
        {
            for (int i = resources.Count-1; i > -1; i--)
            {
                if (resources[i].Tag.Equals(tag)) { 
                    resources.RemoveAt(i);
                    return; // In this implementation, tags should not be repeated, if this is allowed, then to remove all values ​​with a given tag, just comment out this return
                }
            }
        }
    }
}