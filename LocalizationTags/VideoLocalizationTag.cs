using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    public class VideoLocalizationTag : LocalizationTag
    {
        [SerializeField] private VideoPlayer[] players;

        protected override void SetResource(Resource resource)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i]) { players[i].clip = (VideoClip)resource.Data; }
            }
        }

        protected override Resource GetResource()
        {
            VideoClip clip = null;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i]) { clip = players[i].clip; break; }
            }
            return new VideoResource(clip);
        }
    }
}