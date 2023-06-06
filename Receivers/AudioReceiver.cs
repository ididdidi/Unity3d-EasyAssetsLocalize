using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class AudioReceiver : LocalizationReceiver
    {
        public override System.Type Type { get => typeof(AudioSource); }

        public AudioReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Behaviour component, object resource) => ((AudioSource)component).clip = (AudioClip)resource;
    }
}