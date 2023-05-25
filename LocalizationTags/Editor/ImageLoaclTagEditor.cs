using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Displays <see cref="ImageLocalizationTag"/> data in the inspector.S
    /// </summary>
    [CustomEditor(typeof(ImageLocalizationTag))]
    public class ImageLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Image> { }
}