using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LocalizationResource
    {
        [SerializeField] private string tag;
        [SerializeField] private Data data;

        public string Tag { get => tag; }

        public Data Data { get => data; }

        public LocalizationResource(string tag, Data data)
        {
            this.tag = tag;
            this.data = data;
        }
    }
}