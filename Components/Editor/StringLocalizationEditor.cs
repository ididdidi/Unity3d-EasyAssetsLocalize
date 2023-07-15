using UnityEditor;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof(StringLocalization))]
    public class StringLocalizationEditor : LocalizationComponentEditor { }
}