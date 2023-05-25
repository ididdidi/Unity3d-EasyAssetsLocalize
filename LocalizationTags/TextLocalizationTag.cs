using UnityEngine.UI;

namespace ResourceLocalization
{
    /// <summary>
    /// Tag for the <see cref="Text"/> type of the receiving <see cref="string"/>.
    /// </summary>
    public class TextLocalizationTag : ReceiversLocalizationTag<Text>
    {
        protected override void SetResource(Text reciver, Resource resource) => reciver.text = (string)resource.Data;

        protected override Resource GetResource(Text reciver) => new TextResource(reciver.text);
    }
}