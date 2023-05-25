using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Tag for the <see cref="AudioSource"/> type of the receiving <see cref="AudioClip"/>.
    /// </summary>
    public class AudioLocalizationTag : ReceiversLocalizationTag<AudioSource>
    {
        protected override void SetResource(AudioSource reciver, Resource resource) => (reciver).clip = (AudioClip)resource.Data;

        protected override Resource GetResource(AudioSource reciver) => new AudioResource(reciver.clip);
    }
}