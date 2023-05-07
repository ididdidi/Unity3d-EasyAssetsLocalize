using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class FontLocalizationTag : ReceiversLocalizationTag<Text>
    {
        protected override void SetResource(Text reciver, Resource resource) => (reciver).font = (Font)resource.Data;

        protected override Resource GetResource(Text reciver) => new FontResource((reciver).font);
    }
}