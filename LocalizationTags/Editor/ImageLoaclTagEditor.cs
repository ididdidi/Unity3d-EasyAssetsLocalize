using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(ImageLocalizationTag))]
    public class ImageLoaclTagEditor : ReceiversLocalizationTagEditor<UnityEngine.UI.Image> { }
}