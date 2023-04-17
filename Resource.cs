using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public abstract class Resource
    {
        [SerializeField] protected string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private string name;

        public string ID { get => id; }
        public string Name { get => name; }
        public abstract System.Type Type { get; }
        public abstract object Data { get; set; }

        protected Resource(string name) => this.name = name;

        public abstract Resource Clone();
    }

    [System.Serializable]
    public class TextResource : Resource
    {
        [SerializeField] private string data;
        public override System.Type Type { get => typeof(string); }
        public override object Data { get => data; set => data = (string)value; }

        public TextResource(string name, string text) : base(name) => data = text;

        public override Resource Clone()
        {
            var clone = new TextResource(Name, data);
            clone.id = this.id;
            return clone;
        }
    }


    [System.Serializable]
    public class ImageResource : Resource
    {
        [SerializeField] private Texture2D data;
        public override System.Type Type { get => typeof(Texture2D); }
        public override object Data { get => data; set => data = (Texture2D)value; }

        public ImageResource(string name, Texture2D texture) : base(name) => data = texture;

        public override Resource Clone()
        {
            var clone = new ImageResource(Name, data);
            clone.id = this.id;
            return clone;
        }
    }
}