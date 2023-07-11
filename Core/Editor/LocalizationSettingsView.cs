using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSettingsView
    {
        private readonly Color background = new Color(0.22f, 0.22f, 0.22f);
        private LocalizationStorageWindow window;

        public LocalizationSettingsView(LocalizationStorageWindow window)
        {
            this.window = window ?? throw new System.ArgumentNullException(nameof(window));
        }

        public void OnGUI(Rect position)
        {
            EditorGUI.DrawRect(position, background);
            GUI.Label(position, GUIContent.none, "grey_border");

            GUILayout.BeginArea(position);
            GUILayout.Space(2);
            GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Settings", "AM MixerHeader");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Back")) { window.ShowSettings = false; }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}