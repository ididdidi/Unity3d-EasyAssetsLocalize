using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof(SpriteLocalization)), TypeMetadata(typeof(Sprite), "cs Script Icon")]
    public class SpriteLocalizationEditor : LocalizationComponentEditor { }
}