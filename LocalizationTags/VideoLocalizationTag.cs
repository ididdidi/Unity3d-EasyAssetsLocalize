using UnityEngine.Video;

namespace ResourceLocalization
{
    /// <summary>
    /// Tag for the <see cref="VideoPlayer"/> type of the receiving <see cref="VideoClip"/>.
    /// </summary>
    public class VideoLocalizationTag : ReceiversLocalizationTag<VideoPlayer>
    {
        protected override void SetResource(VideoPlayer reciver, Resource resource) 
        {
            if (reciver) reciver.clip = (VideoClip)resource.Data;
        }

        protected override Resource GetResource(VideoPlayer reciver) => new VideoResource(reciver? reciver.clip : null);
    }
}