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
        private void OnEnable()
        {
            Component = target as LocalizationComponent;
            SetTag(Component.ID);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawLocalization();
        }

        private void SelectTag(LocalizationTag tag) 
        {
            if (!LocalizationManager.LocalizationStorage.ContainsLocalizationTag(tag))
            {
                LocalizationManager.LocalizationStorage.AddLocalizationTag(tag);
            }
            Component.ID = tag.ID;
            SetTag(tag.ID);
            EditorUtility.SetDirty(Component);
        }
        
        private void SetTag(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Tag = LocalizationManager.LocalizationStorage.GetLocalizationTag(id);
            }
        }

        private void DrawLocalization()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Localization Tag");
            var buttonText = string.IsNullOrEmpty(Tag?.Name) ? "None" : Tag.Name;
            if (GUILayout.Button(buttonText, EditorStyles.popup))
            {
                var searchView = new SearchTreeView(new LocalizationSearchProvider(LocalizationManager.LocalizationStorage, SelectTag, Component.Type));
                DropDownWindow.Show(searchView, GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
            }
            GUILayout.EndHorizontal();

            if (Tag != null)
            {
                Tag.Name = EditorGUILayout.TextField("Name", Tag.Name);
                LocalizationTagView.DrawResources(Tag); 
            }
        }
    }
}