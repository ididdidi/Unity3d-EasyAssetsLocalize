using UnityEditor;

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

        private void OnEnable() { controller = target as LocalizationController; }

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
        }
    }
}
