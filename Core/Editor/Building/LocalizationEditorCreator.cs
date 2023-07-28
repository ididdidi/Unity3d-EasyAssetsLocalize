public class LocalizationEditorPrototype
{
    private System.Type type;

    public LocalizationEditorPrototype(System.Type type)
    {
        this.type = type ?? throw new System.ArgumentNullException(nameof(type));
    }

    public string Code
    {
        get => $@"
using UnityEditor;

namespace ResourceLocalization
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({type.Name}Localization)), TypeMetadata(typeof({type.FullName}))]
    public class {type.Name}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";
    }
}
