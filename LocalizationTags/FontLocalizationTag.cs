using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class FontLocalizationTag : LocalizationTag
    {
        [SerializeField] private Text textView;

        public override Resource Resource { get => new FontResource(textView.font); set => textView.font = (Font)value.Data; }
    }
}