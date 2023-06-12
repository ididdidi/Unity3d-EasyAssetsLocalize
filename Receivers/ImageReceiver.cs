using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    [System.Serializable]
    public class ImageReceiver : LocalizationReceiver
    {
        public override System.Type[] Types { get => new System.Type[] { typeof(Image), typeof(SpriteRenderer) }; }
        
        public ImageReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Component component, object resource)
        {
            var texture = resource as Texture2D;
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            if (component is Image image)
            {
                image.sprite = sprite;
            }
            else if(component is SpriteRenderer renderer)
            {
                renderer.sprite = sprite;
            }
        }
    }
}
