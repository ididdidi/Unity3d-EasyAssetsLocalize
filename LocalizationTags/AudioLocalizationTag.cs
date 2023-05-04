using UnityEngine;

namespace ResourceLocalization
{
    public class AudioLocalizationTag : LocalizationTag
    {
        [SerializeField] private AudioSource[] sources;

        protected override void SetResource(Resource resource)
        {
            for(int i=0; i < sources.Length; i++)
            {
                if (sources[i]) { sources[i].clip = (AudioClip)resource.Data; }
            }
        }

        protected override Resource GetResource()
        {
            AudioClip clip = null;
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i]) { clip = sources[i].clip; break; }
            }
            return new AudioResource(clip);
        }
    }
}