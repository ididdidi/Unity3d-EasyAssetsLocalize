using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class ImageLocalizationTag : ReceiversLocalizationTag<Image>
    {
        protected override void SetResource(Image reciver, Resource resource)
        {
            var texture = (Texture2D)resource.Data;
            (reciver).sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        protected override Resource GetResource(Image reciver)
        {
            var image = reciver;
            if (image && image.sprite) { return new ImageResource(image.sprite.texture); }
            else return new ImageResource(null);
        }
    }
}