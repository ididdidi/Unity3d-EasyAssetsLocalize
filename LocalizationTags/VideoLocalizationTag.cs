using UnityEngine;
using UnityEngine.Video;

namespace ResourceLocalization
{
    public class VideoLocalizationTag : LocalizationTag
    {
        [SerializeField] private VideoPlayer player;

        public override Resource Resource { get => new VideoResource(player.clip); set => player.clip = (VideoClip)value.Data; }
    }
}