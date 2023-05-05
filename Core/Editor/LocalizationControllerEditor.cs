using UnityEditor;

namespace ResourceLocalization
{
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : Editor
    {
        private LocalizationController controller;
        private ReorderableTagList receiverList;
        private void OnEnable()
        {
            controller = target as LocalizationController;
            receiverList = new ReorderableTagList(controller.Receivers, controller.LocalizationStorage);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (receiverList != null)
            {
                receiverList.DoLayoutList();
            }
        }
    }
}
