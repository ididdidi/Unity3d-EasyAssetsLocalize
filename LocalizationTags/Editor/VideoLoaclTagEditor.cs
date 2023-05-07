using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(VideoLocalizationTag))]
    public class VideoLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.Video.VideoPlayer> { }
}