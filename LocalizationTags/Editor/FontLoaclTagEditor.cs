using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Displays <see cref="FontLocalizationTag"/> data in the inspector.S
    /// </summary>
    [CustomEditor(typeof(FontLocalizationTag))]
    public class FontLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Text> { }
}