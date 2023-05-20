using UnityEngine.Video;

namespace ResourceLocalization
{
    public class VideoLocalizationTag : ReceiversLocalizationTag<VideoPlayer>
    {
        protected override void SetResource(VideoPlayer reciver, Resource resource) 
        {
            if (reciver) reciver.clip = (VideoClip)resource.Data;
        }

        protected override Resource GetResource(VideoPlayer reciver) => new VideoResource(reciver? reciver.clip : null);
    }
}