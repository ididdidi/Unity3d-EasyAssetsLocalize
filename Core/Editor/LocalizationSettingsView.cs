using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSettingsView : IView
    {
        private GUIStyle style;

        private LanguagesListView languagesList;
        private TypesListView typesList;
        private Vector2 scrollPosition = default;
        private System.Action onCloseButton;

        public LocalizationSettingsView(System.Action onCloseButton)
        {
            this.onCloseButton = onCloseButton ?? throw new System.ArgumentNullException(nameof(onCloseButton));
            languagesList = new LanguagesListView(LocalizationManager.Storage);
            typesList = new TypesListView();
        }

        public void OnGUI(Rect position)
        {
            if (style == null) { style = new GUIStyle("AM MixerHeader"); }

            var content = GUIContent.none;
            GUI.Label(position, content, "grey_border");

            GUILayout.BeginArea(position);
            GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);

            content = new GUIContent(EditorGUIUtility.IconContent("tab_prev@2x").image, "Back");
            if (GUILayout.Button(content, style, GUILayout.Width(20f), GUILayout.Height(20f))) {
                Close();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Propertyes", style);
            GUILayout.FlexibleSpace();

            content = new GUIContent(EditorGUIUtility.IconContent("_Help@2x").image, "Help");
            if (GUILayout.Button(content, style, GUILayout.Width(20f), GUILayout.Height(20f)))
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
            onCloseButton();
            EditorGUI.FocusTextInControl(null);
        }
    }
}