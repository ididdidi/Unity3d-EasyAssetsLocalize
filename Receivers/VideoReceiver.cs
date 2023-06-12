using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    [System.Serializable]
    public class VideoReceiver : LocalizationReceiver
    {
        public override System.Type[] Types { get => new System.Type[] { typeof(VideoPlayer) }; }

        public VideoReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        protected override void SetLocalization(Component component, object resource) => ((VideoPlayer)component).clip = (VideoClip)resource;
    }
}