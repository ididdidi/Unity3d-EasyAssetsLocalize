using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : Editor
    {
        private LocalizationController controller;
        private ReorderableTagList receiverList;

        private void OnEnable() { controller = target as LocalizationController; }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!controller.LocalizationStorage)
            { 
                EditorGUILayout.HelpBox($"The {controller.LocalizationStorage} field must not be empty!", MessageType.Error); return;
            }

            if (receiverList == null) { receiverList = new ReorderableTagList(controller.Receivers, controller.LocalizationStorage); }
            else { receiverList.DoLayoutList(); }
        }
    }
}
