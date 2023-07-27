
using UnityEditor;
using UnityEngine;

public class LocalizationEditorCreator : ClassCreator
{
    private string path;
    private object defaultValue;

    public LocalizationEditorCreator(string path, object defaultValue)
    {
        this.path = path ?? throw new System.ArgumentNullException(nameof(path));
        this.defaultValue = defaultValue ?? throw new System.ArgumentNullException(nameof(defaultValue));
    }

    public override void CreateClass()
    {
        var type = defaultValue.GetType(); 
        string defVal = defaultValue is string ? (string)defaultValue : AssetDatabase.GetAssetPath((Object)defaultValue);

        var code = (type.Namespace.Equals("UnityEngine") ? "" : $"using {type.Namespace};") + $@"
using UnityEngine;
using UnityEditor;

namespace ResourceLocalization
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({type.Name}Localization)), TypeMetadata(typeof({type.Name}), ""{defVal}"")]
    public class {type.Name}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";

        CreateClass(type.Name + "LocalizationEditor", path + "Editor/", code);
    }
}
