using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class FontResource : Resource
    {
        [SerializeField] private Font data;
        public override System.Type Type { get => typeof(Font); }
        public override object Data { get => data; set => data = (Font)value; }

        public FontResource(Font texture) => data = texture;

        public override Resource Clone() => new FontResource(data);
    }
}