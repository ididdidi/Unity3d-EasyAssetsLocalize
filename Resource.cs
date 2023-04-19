using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public abstract class Resource
    {
        public abstract System.Type Type { get; }
        public abstract object Data { get; set; }

        public abstract Resource Clone();
    }

    [System.Serializable]
    public class TextResource : Resource
    {
        [SerializeField] private string data;
        public override System.Type Type { get => typeof(string); }
        public override object Data { get => data; set => data = (string)value; }

        public TextResource(string text) => data = text;

        public override Resource Clone() => new TextResource(data);
    }


    [System.Serializable]
    public class ImageResource : Resource
    {
        [SerializeField] private Texture2D data;
        public override System.Type Type { get => typeof(Texture2D); }
        public override object Data { get => data; set => data = (Texture2D)value; }

        public ImageResource(Texture2D texture) => data = texture;

        public override Resource Clone() => new ImageResource(data);
    }
}