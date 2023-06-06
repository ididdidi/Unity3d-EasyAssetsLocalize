using UnityEngine;
using UnityEngine.UI;

namespace ResourceLocalization
{
    [System.Serializable]
    public class TextReceiver : LocalizationReceiver
    {
        public override System.Type Type { get => typeof(Text); }

        public TextReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Behaviour component, object resource) => ((Text)component).text = resource as string;
    }
}