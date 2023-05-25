using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Encapsulates text resources
    /// </summary>
    [System.Serializable]
    public class TextResource : Resource
    {
        [SerializeField] private string data;
        public override System.Type Type { get => typeof(string); }
        public override object Data { get => data; set => data = (string)value; }

        public TextResource(string text) => data = text;

        public override Resource Clone() => new TextResource(data);
    }
}