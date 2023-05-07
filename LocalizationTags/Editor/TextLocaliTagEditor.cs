using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(TextLocalizationTag))]
    public class TextLocalTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Text> { }
}