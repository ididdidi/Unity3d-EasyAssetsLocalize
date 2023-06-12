using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    [System.Serializable]
    public class TextReceiver : LocalizationReceiver
    {
        public override System.Type[] Types { get => new System.Type[] { typeof(Text)}; }

        public TextReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Component component, object resource) => ((Text)component).text = resource as string;
    }
}