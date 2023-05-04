using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class ImageLocalizationTag : LocalizationTag
    {
        [SerializeField] private Image[] images;

        protected override void SetResource(Resource resource)
        {
            var texture = resource.Data as Texture2D;

            for (int i=0; i < images.Length; i++)
            {
                if (images[i]) { images[i].sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f)); }
            }
        }

        protected override Resource GetResource()
        {
            Texture2D texture = null;
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] && images[i].sprite) { texture = images[i].sprite.texture; break; }
            }
            return new ImageResource(texture);
        }
    }
}