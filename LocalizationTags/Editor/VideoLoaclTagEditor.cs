using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Displays <see cref="VideoLocalizationTag"/> data in the inspector.S
    /// </summary>
    [CustomEditor(typeof(VideoLocalizationTag))]
    public class VideoLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.Video.VideoPlayer> { }
}