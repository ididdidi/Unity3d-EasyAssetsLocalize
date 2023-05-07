using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(FontLocalizationTag))]
    public class FontLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Text> { }
}