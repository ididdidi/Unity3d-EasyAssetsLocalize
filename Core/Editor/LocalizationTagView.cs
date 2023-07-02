using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    public class LocalizationTagView : IEditorView
    {
        private LocalizationTag Tag { get; }
        public LocalizationTagView(LocalizationTag tag) => Tag = tag;

        public void OnGUI(IContext context)
        {
            DrawResources(Tag);
        }

        public static void DrawResources(LocalizationTag tag)
        {
            var rect = GUILayoutUtility.GetRect(100f, GetHeight(tag));

            EditorGUI.BeginChangeCheck();
            tag.Name = EditorGUI.TextField(rect, "Localization name", tag.Name);
            var controlID = ExtendedEditorGUI.GetLastControlId();
            controlID.ReleaseOnClick();

            var languages = LocalizationManager.LocalizationStorage.Languages;
            var isString = tag.Type.IsAssignableFrom(typeof(string));
            for (int i = 0; i < tag.Resources.Count; i++)
            {
                if (isString)
                {
                    EditorGUILayout.LabelField(languages[i].Name);
                    tag.Resources[i].Data = EditorGUILayout.TextArea((string)tag.Resources[i].Data, GUILayout.Height(50f));
                }
                else
                {
                    tag.Resources[i].Data = EditorGUILayout.ObjectField(languages[i].Name, (Object)tag.Resources[i].Data, tag.Type, false);
                }
            }
        }

        public static float GetHeight(LocalizationTag tag)
        {
            var height = EditorGUIUtility.singleLineHeight;

            var isString = tag.Type.IsAssignableFrom(typeof(string));
            for (int i = 0; i < tag.Resources.Count; i++)
            {
                if (isString) { height += EditorGUIUtility.singleLineHeight; }
                height += 50f;
            }
            return height;
        }
    }
}