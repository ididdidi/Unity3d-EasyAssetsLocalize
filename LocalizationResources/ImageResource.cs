using UnityEngine;

namespace ResourceLocalization
{
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