using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : Editor
    {
        private LocalizationController controller;
        private ReorderableReceiverList receiverList;
        private void OnEnable()
        {
            controller = target as LocalizationController;
            receiverList = new ReorderableReceiverList(controller.Receivers, controller.LocalizationStorage.Localizations);
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
