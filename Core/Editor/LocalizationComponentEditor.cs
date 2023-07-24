using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization component.
    /// </summary>
    [CustomEditor(typeof(LocalizationComponent))]
    public class LocalizationComponentEditor : Editor
    {
        private LocalizationComponent Component { get; set; }
        private LocalizationTag Tag { get; set; }
        private LocalizationStorage Storage { get => LocalizationManager.LocalizationStorage; }

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

        private bool SelectTag(LocalizationTag tag) 
        {
            if (!Storage.ContainsLocalizationTag(tag))
            {
                Storage.AddLocalizationTag(tag);
            }
            Component.ID = tag.ID;
            SetTag(tag.ID);
            EditorUtility.SetDirty(Component);
            return true;
        }
        
        private void SetTag(string id)
        {
            if (!string.IsNullOrEmpty(id)) { Tag = LocalizationManager.LocalizationStorage.GetLocalizationTag(id); }
        }

        private void DrawLocalization()
        {
            if (Tag != null)
            {
                EditorGUI.BeginChangeCheck();
                Tag.Name = EditorGUILayout.TextField("Localization name", Tag.Name);
                LocalizationView.DrawResources(Tag, LocalizationManager.Languages, GUILayout.Height(50f));
                if (EditorGUI.EndChangeCheck()) { LocalizationManager.LocalizationStorage?.ChangeVersion(); }
            }

            if (ExtendedEditor.CenterButton(Tag == null ? "Set Localization" : "Change Localization")) { SetLocalization(); }
        }

        private void SetLocalization()
        {
            SearchDropDownWindow.Show(new LocalizationSearchProvider(Storage, 
                (data) => { if (data is LocalizationTag tag) return SelectTag(tag); else return false; }, null, Component.Type));
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