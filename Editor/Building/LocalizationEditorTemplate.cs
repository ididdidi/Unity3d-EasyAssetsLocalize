namespace EasyAssetsLocalize
{
    /// <summary>
    /// A class that contains a code template for creating an Editor for component.
    /// </summary>
    public class LocalizationEditorTemplate
    {
        private System.Type type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">resource type</param>
        public LocalizationEditorTemplate(System.Type type)
        {
            this.type = type ?? throw new System.ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Code to create class file.
        /// </summary>
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
