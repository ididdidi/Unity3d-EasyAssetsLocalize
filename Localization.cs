using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public partial class Localization
    {
        [SerializeField] private string language;
        [SerializeReference] private List<Resource> resources;

        public string Language { get => language; set => language = value; }

        public Localization(string language)
        {
            this.language = language;
            resources = new List<Resource>();
        }
               
        public Dictionary<string, Resource> Dictionary
        {
            get
            {
                var dictionary = new Dictionary<string, Resource>();
                for (int i = 0; i < resources.Count; i++)
                {
                    dictionary.Add(resources[i].ID, resources[i]);
                }
                return dictionary;
            }
        }

        public bool Conteins(string id)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].ID.Equals(id)) { return true; }
            }
            return false;
        }

        public void SetValue(Resource resource)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].ID.Equals(resource.ID)) { resources[i] = resource; return; }
            }
            resources.Add(resource);
        }

     //   public object GetValue(string id)
     //   {
     //       for (int i = 0; i < resources.Count; i++)
     //       {
     //           if (resources[i].ID.Equals(id)) { return resources[i].Data; }
     //       }
     //       throw new System.ArgumentException($"{GetType()}: No resources found for {id}");
     //   }

        public System.Type GetDataType(string id)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].ID.Equals(id)) { return resources[i].Type; }
            }
            throw new System.ArgumentException($"{GetType()}: No resources found for {id}");
        }

        public Resource GetResource(int index)
        {
            if (index > -1 && index < resources.Count) { return resources[index]; }
            throw new System.ArgumentException($"{GetType()}: No resources found by {index}");
        }
        
        public void Remove(string id)
        {
            for (int i = resources.Count-1; i > -1; i--)
            {
                if (resources[i].ID.Equals(id)) { 
                    resources.RemoveAt(i);
                    return; // In this implementation, names should not be repeated, if this is allowed, then to remove all values ​​with a given name, just comment out this return
                }
            }
        }

        public void RemoveAt(int index) => resources.RemoveAt(index);

        public void Insert(int index, Resource resource)
        {
            if (index < 0 || index >= resources.Count) { throw new System.ArgumentException($"{GetType()}: The index must have a value between 0 and {resources.Count}"); }
            if (Conteins(resource.ID)) { throw new System.ArgumentException($"{GetType()}: An item with {resource.ID} has already been added."); }

            resources.Insert(index, resource);
        }
    }
}