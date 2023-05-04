using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    public class FontLocalizationTag : LocalizationTag
    {
        [SerializeField] private Text[] textViews;

        protected override void SetResource(Resource resource)
        {
            for (int i = 0; i < textViews.Length; i++)
            {
                if (textViews[i]) { textViews[i].font = (Font)resource.Data; }
            }
        }

        protected override Resource GetResource()
        {
            Font font = null;
            for (int i = 0; i < textViews.Length; i++)
            {
                if (textViews[i]) { font = textViews[i].font; break; }
            }
            return new FontResource(font);
        }
    }
}