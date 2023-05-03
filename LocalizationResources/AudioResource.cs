using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class AudioResource : Resource
    {
        [SerializeField] private AudioClip data;
        public override System.Type Type { get => typeof(AudioClip); }
        public override object Data { get => data; set => data = (AudioClip)value; }

        public AudioResource(AudioClip clip) => data = clip;

        public override Resource Clone() => new AudioResource(data);
    }
}