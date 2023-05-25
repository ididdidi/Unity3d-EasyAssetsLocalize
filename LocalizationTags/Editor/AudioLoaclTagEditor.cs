using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Displays <see cref="AudioLocalizationTag"/> data in the inspector.S
    /// </summary>
    [CustomEditor(typeof(AudioLocalizationTag))]
    public class AudioLoaclTagEditor : ReceiversLocalizationTagEditor<AudioSource> { }
}