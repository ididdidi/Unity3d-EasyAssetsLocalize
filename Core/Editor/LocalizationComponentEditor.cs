﻿using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    /// <summary>
    /// Class for displaying localization component.
    /// </summary>
    [CustomEditor(typeof(LocalizationComponent))]
    public class LocalizationComponentEditor : Editor
    {
        private LocalizationComponent Component { get; set; }
        private Localization Tag { get; set; }
        private LocalizationStorage Storage { get => LocalizationManager.Storage; }

        private void OnEnable()
        {
            Component = target as LocalizationComponent;
            SetTag(Component.ID);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawLocalization();
            DrawHandler();
        }

        private bool SelectTag(Localization tag) 
        {
            if (!Storage.ContainsLocalization(tag))
            {
                Storage.AddLocalization(tag);
            }
            Component.ID = tag.ID;
            SetTag(tag.ID);
            EditorUtility.SetDirty(Component);
            return true;
        }
        
        private void SetTag(string id)
        {
            if (!string.IsNullOrEmpty(id)) { Tag = LocalizationManager.Storage.GetLocalization(id); }
        }

        private void DrawLocalization()
        {
            if (Tag != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.BeginDisabledGroup(Tag.IsDefault);
                Tag.Name = EditorGUILayout.TextField("Localization name", Tag.Name);
                LocalizationView.DrawResources(Tag, LocalizationManager.Languages, GUILayout.Height(50f));
                EditorGUI.EndDisabledGroup();
                if (EditorGUI.EndChangeCheck()) { LocalizationManager.Storage?.ChangeVersion(); }
            }

            if (ExtendedEditor.CenterButton(Tag == null ? "Set Localization" : "Change Localization")) { SetLocalization(); }
        }

        private void SetLocalization()
        {
            SearchDropDownWindow.Show(new LocalizationSearchProvider(Storage, 
                (data) => { if (data is Localization tag) return SelectTag(tag); else return false; }, null, Component.Type));
        }


        private SerializedProperty handler;
        private void DrawHandler()
        {
            serializedObject.Update();
            if (Tag != null)
            {
                if (handler == null) { handler = serializedObject.FindProperty("handler"); }
                if (handler != null) { EditorGUILayout.PropertyField(handler); }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}