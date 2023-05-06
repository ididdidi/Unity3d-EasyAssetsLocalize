 using UnityEngine;

namespace ResourceLocalization
{
    public abstract class ResourceLocalizationTagEditor<T> : LocalizationTagEditor where T : Object
    {
        private ResourceLocalizationTag<T> tag;
        private bool reciversfoldout;

        public override void OnEnable()
        {
            base.OnEnable();
            tag = target as ResourceLocalizationTag<T>;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            tag.recivers = tag.recivers.ArrayFields("Recivers", ref reciversfoldout);
        }
    }
}