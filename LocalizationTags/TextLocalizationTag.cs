using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class TextLocalizationTag : LocalizationTag
    {
        [SerializeField] private Text textView;
        public override Resource Resource 
        {
            get => new TextResource(textView?.text); 
            set => textView.text = (string)value.Data; 
        }
    }
}