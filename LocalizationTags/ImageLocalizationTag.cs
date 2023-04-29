using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class ImageLocalizationTag : LocalizationTag
    {
        [SerializeField] private Image image;

        public override Resource Resource { get => new ImageResource(GetTexture()); set => SetTexture((Texture2D)value.Data); }

        private void SetTexture(Texture2D texture)
        {
            image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private Texture2D GetTexture()
        {
            if(image && image.sprite) { return image.sprite.texture; }
            return null;
        }
    }
}