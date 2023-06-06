using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
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

            EditorGUI.BeginChangeCheck();
            DrawLocalisationTags();
            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(controller);
                EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
            }

            if (AddLocalizationButton()) { AddLocalization(); };
        }

        private void DrawLocalisationTags()
        {
            for(int i=0; i < controller.LocalizationReceivers.Count; i++)
            {
                DrawLocalisationTag(controller.LocalizationReceivers[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        private void DrawLocalisationTag(LocalizationReceiver receiver)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(receiver.Name);
            if(RemoveLocalizationButton()) { RemoveLocalization(receiver); }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            receiver.Components = ExtendedEditorGUI.ArrayFields(receiver.Components, "Components", ref receiver.open, true, receiver.Type);
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
            var window = (SetLocalizationTagWindow)EditorWindow.GetWindow(typeof(SetLocalizationTagWindow), true, controller.name);
            window.LocalizationController = controller;
        }

        private bool RemoveLocalizationButton()
        {
            return GUILayout.Button(EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove"), "SearchCancelButton");
        }

        private void RemoveLocalization(LocalizationReceiver receiver)
        {
            controller.RemoveLocalizationTag(receiver);
        }
    }
}
