using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class AudioReceiver : LocalizationReceiver
    {
        public override System.Type[] Types { get => new System.Type[] { typeof(AudioSource) }; }

        public AudioReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Component component, object resource) => ((AudioSource)component).clip = (AudioClip)resource;
    }
}