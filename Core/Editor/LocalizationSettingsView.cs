using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSettingsView
    {
        private GUIStyle style;

        private LocalizationStorageWindow window;
        private LanguagesListView languagesList;
        private TypesListView typesList;
        private Vector2 scrollPosition = default;

        public LocalizationSettingsView(LocalizationStorageWindow window)
        {
            this.window = window ?? throw new System.ArgumentNullException(nameof(window));
            languagesList = new LanguagesListView(LocalizationManager.Storage);
            typesList = new TypesListView();
        }

        public void OnGUI(Rect position)
        {
            if(style == null) { style = new GUIStyle("AM MixerHeader"); }

            var content = GUIContent.none;
            GUI.Label(position, content, "grey_border");

            GUILayout.BeginArea(position);
            GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);

            content = new GUIContent(EditorGUIUtility.IconContent("tab_prev@2x").image, "Back");
            if (GUILayout.Button(content, style, GUILayout.Height(20f))) {
                Close();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Propertyes", style);
            GUILayout.FlexibleSpace();

            content = new GUIContent(EditorGUIUtility.IconContent("_Help@2x").image, "Help");
            if (GUILayout.Button(content, style, GUILayout.Height(20f)))
            {
                // Реализовать вывод справки
            }
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, EditorStyles.inspectorDefaultMargins);
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            languagesList.DoLayoutList();
            GUILayout.EndVertical();

            GUILayout.Space(4);

            GUILayout.BeginVertical();
            typesList.DoLayoutList();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            var curentEvent = Event.current;
            if (curentEvent.type == EventType.KeyDown && curentEvent.keyCode == KeyCode.Escape)
            {
                Close();
                curentEvent.Use();
            }
        }

        public void Close()
        {
            languagesList.index = typesList.index  = -1;
            window.ShowSettings = false;
            EditorGUI.FocusTextInControl(null);
        }
    }
}