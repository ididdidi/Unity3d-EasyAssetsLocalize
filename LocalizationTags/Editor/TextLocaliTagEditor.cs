using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(TextLocalizationTag))]
    public class TextLocalTagEditor : ResourceLocalizationTagEditor<UnityEngine.UI.Text> { }
}