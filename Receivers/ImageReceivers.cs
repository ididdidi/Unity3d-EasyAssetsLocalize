using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    [System.Serializable]
    public class ImageReceiver : LocalizationReceiver
    {
        public override System.Type Type { get => typeof(Image); }
        
        public ImageReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        public override void SetLocalization(Behaviour component, object resource)
        {
            var texture = resource as Texture2D;
            ((Image)component).sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}
