using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityExtended;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization controller fields and tags in a reorderable list.
    /// </summary>
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : Editor
    {
        private LocalizationController controller;

        private void OnEnable()
        { 
            controller = target as LocalizationController;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Throws an error in the inspector if a localization store has not been added to the corresponding field.
            if (!controller.LocalizationStorage)
            { 
                EditorGUILayout.HelpBox($"The {controller.LocalizationStorage} field must not be empty!", MessageType.Error); return;
            }

            DrawLocalisationTags();

            if(AddLocalizationButton()) { AddLocalization(); };
        }

        private void DrawLocalisationTags()
        {
            for(int i=0; i < controller.LocalizationTags.Count; i++)
            {
                DrawLocalisationTag(controller.LocalizationTags[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        private void DrawLocalisationTag(LocalizationTag tag)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(tag.Name);
            if(RemoveLocalizationButton()) { RemoveLocalization(tag); }
            GUILayout.EndHorizontal();
            
            EditorGUI.indentLevel++;
            tag.Receivers = ExtendedEditorGUI.ArrayFields(tag.Receivers, "Receivers", ref tag.open, true, typeof(Image));
            EditorGUI.indentLevel--;

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Button to display localization choice window.
        /// </summary>
        private bool AddLocalizationButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var result = GUILayout.Button("Choice localization", GUILayout.Width(240f), GUILayout.Height(24f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            return result;
        }

        private void AddLocalization()
        {
            var window = (LocalizationChoiceWindow)EditorWindow.GetWindow(typeof(LocalizationChoiceWindow), true, controller.name);
            window.LocalizationController = controller;
        }

        private bool RemoveLocalizationButton()
        {
            return GUILayout.Button(EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove"), "SearchCancelButton");
        }

        private void RemoveLocalization(LocalizationTag tag)
        {
            controller.RemoveLocalizationTag(tag);
        }
    }
}
