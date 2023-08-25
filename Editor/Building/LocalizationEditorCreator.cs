namespace EasyAssetsLocalize
{
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

namespace {GetType().Namespace}
{{
    /// <summary>
    /// Class for displaying localization fields.
    /// </summary>
    [CustomEditor(typeof({type.Name}Localization))]
    public class {type.Name}LocalizationEditor : LocalizationComponentEditor {{ }}
}}";
        }
    }
}
