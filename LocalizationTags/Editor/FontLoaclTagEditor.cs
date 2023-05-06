using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(FontLocalizationTag))]
    public class FontLoaclTagEditor : ResourceLocalizationTagEditor<UnityEngine.UI.Text> { }
}