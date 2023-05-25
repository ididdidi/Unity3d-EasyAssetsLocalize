using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Displays <see cref="TextLocalizationTag"/> data in the inspector.S
    /// </summary>
    [CustomEditor(typeof(TextLocalizationTag))]
    public class TextLocalTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Text> { }
}