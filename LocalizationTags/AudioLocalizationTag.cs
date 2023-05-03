using UnityEngine;

namespace ResourceLocalization
{
    public class AudioLocalizationTag : LocalizationTag
    {
        [SerializeField] private AudioSource source;

        public override Resource Resource { get => new AudioResource(source.clip); set => source.clip = (AudioClip)value.Data; }
    }
}