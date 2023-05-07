using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    [CustomEditor(typeof(AudioLocalizationTag))]
    public class AudioLoaclTagEditor : ReceiversLocalizationTagEditor<AudioSource> { }
}