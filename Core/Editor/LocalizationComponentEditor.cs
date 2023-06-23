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
        LocalizationComponent component;
        private void OnEnable()
        {
            component = target as LocalizationComponent;
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
                DropDownWindow.Show(searchView, GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
            }
            GUILayout.EndHorizontal();
        }
    }
}