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
                    dictionary.Add(resources[i].Name, resources[i].Data);
                }
                return dictionary;
            }
        }

        public bool Conteins(string name)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].Name.Equals(name)) { return true; }
            }
            return false;
        }

        public void SetValue(string name, object value)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].Name.Equals(name)) { resources[i].Data = value; return; }
            }
            resources.Add(new SerializedResources(name, value));
        }

        public object GetValue(string name)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].Name.Equals(name)) { return resources[i].Data; }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {name}");
        }

        public string GetName(int index)
        {
            if (index > -1 && index < resources.Count) { return resources[index].Name; }
            throw new System.ArgumentException($"{GetType()}: No resources found by {index}");
        }

        public object GetValue(int index)
        {
            if (index > -1 && index < resources.Count) { return resources[index].Data; }
            throw new System.ArgumentException($"{GetType()}: No resources found by {index}");
        }

        public void RemoveAt(int index) => resources.RemoveAt(index);
        
        public void Remove(string name)
        {
            for (int i = resources.Count-1; i > -1; i--)
            {
                if (resources[i].Name.Equals(name)) { 
                    resources.RemoveAt(i);
                    return; // In this implementation, names should not be repeated, if this is allowed, then to remove all values ​​with a given name, just comment out this return
                }
            }
        }

        public void Insert(int index, string name, object value)
        {
            if (index < 0 || index >= resources.Count) { throw new System.ArgumentException($"{GetType()}: The index must have a value between 0 and {resources.Count}"); }
            if (Conteins(name)) { throw new System.ArgumentException($"{GetType()}: An item with {name} has already been added."); }

            resources.Insert(index, new SerializedResources(name, value));
        }
    }
}