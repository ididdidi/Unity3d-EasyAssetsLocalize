using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof(StringLocalization)), TypeMetadata(typeof(string), "cs Script Icon")]
    public class StringLocalizationEditor : LocalizationComponentEditor { }
}