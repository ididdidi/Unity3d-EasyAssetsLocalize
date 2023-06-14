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

            if (SetLocalizationTagButton())
            {
                var window = (SetLocalizationTagWindow)EditorWindow.GetWindow(typeof(SetLocalizationTagWindow), true, controller.name);
                window.LocalizationController = controller;
            };
        }

        private void DrawLocalisationTags()
        {
            var buttonSize = EditorGUIUtility.singleLineHeight;
            for (int i=0; i < controller.LocalizationReceivers.Count; i++)
            {
                var rect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                if (ExtendedEditorGUI.CancelButton(new Rect(rect.x + rect.width - buttonSize, rect.y + 2f, buttonSize, buttonSize), "Remove"))
                {
                    Undo.RecordObject(controller, controller.name);
                    controller.RemoveLocalizationReceiver(controller.LocalizationReceivers[i]);
                    return;
                }
                controller.LocalizationReceivers[i].OnInspectorGUI();
                EditorGUILayout.EndVertical();
            }
        }

        /// <summary>
        /// Button to display localization choice window.
        /// </summary>
        private bool SetLocalizationTagButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var result = GUILayout.Button("Set localization tag", GUILayout.Width(240f), GUILayout.Height(24f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            return result;
        }
    }
}
