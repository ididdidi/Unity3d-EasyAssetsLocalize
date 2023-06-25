﻿using UnityEditor;
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
            var language = LocalizationManager.Language.Name;
            var isString = tag.Type.IsAssignableFrom(typeof(string));
            for (int i = 0; i < tag.Resources.Count; i++)
            {
                if (isString)
                {
                    EditorGUILayout.LabelField(language);
                    tag.Resources[i].Data = EditorGUILayout.TextArea((string)tag.Resources[i].Data, GUILayout.Height(50f));
                }
                else
                {
                    tag.Resources[i].Data = EditorGUILayout.ObjectField(language, (Object)tag.Resources[i].Data, tag.Type, false);
                }
            }
        }
    }
}