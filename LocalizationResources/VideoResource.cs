using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    [System.Serializable]
    public class VideoResource : Resource
    {
        [SerializeField] private VideoClip data;
        public override System.Type Type { get => typeof(VideoClip); }
        public override object Data { get => data; set => data = (VideoClip)value; }

        public VideoResource(VideoClip clip) => data = clip;

        public override Resource Clone() => new VideoResource(data);
    }
}