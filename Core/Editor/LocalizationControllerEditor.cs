using UnityEditor;
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
        bool foldout;

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

          //  EditorGUI.BeginChangeCheck();
          //  DrawLocalisationTags();
          //  if(EditorGUI.EndChangeCheck())
          //  {
          //      EditorUtility.SetDirty(controller);
          //      EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
          //  }

            SetLocalizationTagButton();

            serializedObject.Update();
            DrawPropertyArray(serializedObject.FindProperty("receives"), ref foldout);
            serializedObject.ApplyModifiedProperties();
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
        private void SetLocalizationTagButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Set localization tag", GUILayout.Width(240f), GUILayout.Height(24f)))
            {
                SetLocalizationTagWindow.GetInstance(controller, "Set Localization Tag");
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void DrawPropertyArray(SerializedProperty property, ref bool fold)
        {
            if (property == null) { Debug.Log(property); return; }
            fold = EditorGUILayout.Foldout(fold, new GUIContent(
                property.displayName,
                "These are the waypoints that will be used for the moving object's path."), true);
            if (!fold) return;
            var arraySizeProp = property.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, true);

            EditorGUI.indentLevel++;

            for (var i = 0; i < arraySizeProp.intValue; i++)
            {
                EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i));
            }

            EditorGUI.indentLevel--;
        }

    }
}
