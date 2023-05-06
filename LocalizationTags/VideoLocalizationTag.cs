using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    public class VideoLocalizationTag : ResourceLocalizationTag<VideoPlayer>
    {
        protected override void SetResource(VideoPlayer reciver, Resource resource) => reciver.clip = (VideoClip)resource.Data;

        protected override Resource GetResource(VideoPlayer reciver) => new VideoResource(reciver.clip);
    }
}