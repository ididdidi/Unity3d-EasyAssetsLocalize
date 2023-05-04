using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class TextLocalizationTag : LocalizationTag
    {
        [SerializeField] private Text[] textViews;

        protected override void SetResource(Resource resource)
        {
            for (int i = 0; i < textViews.Length; i++)
            {
                if (textViews[i]) { textViews[i].text = (string)resource.Data; }
            }
        }

        protected override Resource GetResource()
        {
            string text = "";
            for (int i = 0; i < textViews.Length; i++)
            {
                if (textViews[i]) { text = textViews[i].text; break; }
            }
            return new TextResource(text);
        }
    }
}