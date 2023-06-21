using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for displaying localization controller fields and tags in a reorderable list.
    /// </summary>
    [CustomEditor(typeof(StringLocalization))]
    public class LocalizationComponentEditor : Editor
    {
        StringLocalization component;
        private void OnEnable()
        {
            component = target as StringLocalization;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Localization Tag");
            var buttonText = string.IsNullOrEmpty(component.Tag.Name)? "None" : component.Tag.Name;
            if (GUILayout.Button(buttonText, EditorStyles.popup))
            {
                var searchView = new SearchTreeView(new LocalizationSearchProvider(component));
                DropDownWindow.Show(GUIUtility.GUIToScreenPoint(Event.current.mousePosition), Vector2.zero, searchView);
            }
            GUILayout.EndHorizontal();
        }
    }
}