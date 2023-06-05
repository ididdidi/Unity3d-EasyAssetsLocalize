using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class TextResource : IResource
    {
        [SerializeField] private string data;

        public object Data { get => data; set => data = (string)value; }
        public TextResource(object data) => Data = data ?? throw new System.ArgumentNullException(nameof(data));

        public IResource Clone() => new TextResource(data);
    }
}
