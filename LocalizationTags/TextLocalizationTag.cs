using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class TextLocalizationTag : ReceiversLocalizationTag<Text>
    {
        protected override void SetResource(Text reciver, Resource resource) => reciver.text = (string)resource.Data;

        protected override Resource GetResource(Text reciver) => new TextResource(reciver.text);
    }
}