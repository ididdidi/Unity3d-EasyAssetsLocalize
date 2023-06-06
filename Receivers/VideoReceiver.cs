using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    [System.Serializable]
    public class VideoReceiver : LocalizationReceiver
    {
        public override System.Type Type { get => typeof(VideoPlayer); }

        public VideoReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Behaviour component, object resource) => ((VideoPlayer)component).clip = (VideoClip)resource;
    }
}