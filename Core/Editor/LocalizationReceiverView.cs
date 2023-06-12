using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    public static class LocalizationReceiverView
    {
        public static void OnInspectorGUI(this LocalizationReceiver receiver)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button(receiver.Name, EditorStyles.label)) Debug.Log(receiver.Name);
            DrawComponents(receiver);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiver"></param>
        private static void DrawComponents(LocalizationReceiver receiver)
        {
            EditorGUI.indentLevel++;
            var rect = EditorGUILayout.BeginVertical();
            receiver.Components = ExtendedEditorGUI.ArrayFields(receiver.Components, "Components", ref receiver.open, true, receiver.Types);
            EditorGUILayout.EndVertical();
            ExtendedEditorGUI.ChecDragAndDrops(rect, receiver.Types);
            EditorGUI.indentLevel--;
        }
    }
}