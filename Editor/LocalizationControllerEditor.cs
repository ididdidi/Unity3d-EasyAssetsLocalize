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
            receiverList = new ReorderableReceiverList(controller.Receivers);
            receiverList.onAddCallback += AddRecever;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(receiverList != null)
            {
                receiverList.DoLayoutList();
            }
        }

        private void AddRecever(UnityEditorInternal.ReorderableList list)
        {
            var receiver = new LocalizationReceiver();
            controller.Receivers.Add(receiver);
            foreach (var localization in controller.LocalizationStorage.Localizations)
            {
                localization.SetValue(receiver.ID, "");
            }
        }
    }
}
