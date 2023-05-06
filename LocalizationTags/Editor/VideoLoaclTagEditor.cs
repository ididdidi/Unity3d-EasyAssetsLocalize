using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(VideoLocalizationTag))]
    public class VideoLoaclTagEditor : ResourceLocalizationTagEditor<UnityEngine.Video.VideoPlayer> { }
}