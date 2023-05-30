using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization controller fields and tags in a reorderable list.
    /// </summary>
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : Editor
    {
        private LocalizationController controller;
        private ReorderableTagList receiverList;

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

            // Display a reorderable list of localization tags
            if (receiverList == null) { receiverList = new ReorderableTagList(controller.LocalizationTags, controller.LocalizationStorage); }
            else { receiverList.DoLayoutList(); }

            DrowButton();
        }

        /// <summary>
        /// Button to display localization choice window.
        /// </summary>
        private void DrowButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Choice localization", GUILayout.Width(240f), GUILayout.Height(24f)))
            {
                var window = (LocalizationChoiceWindow)EditorWindow.GetWindow(typeof(LocalizationChoiceWindow), true, controller.name);
                window.LocalizationStorage = controller.LocalizationStorage;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
