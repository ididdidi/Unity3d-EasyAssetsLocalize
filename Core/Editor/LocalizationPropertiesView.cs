using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public class LocalizationPropertiesView : IEditorView
    {
        private static readonly string helpURL = "https://ididdidi.ru/";
        private LanguagesListView languagesList;
        private TypesListView typesList;
        private Vector2 scrollPosition;

        public System.Action OnBackButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onCloseButton">Callback to close view</param>
        public LocalizationPropertiesView()
        {
            languagesList = new LanguagesListView(LocalizationManager.Storage);
            typesList = new TypesListView();
        }

        public void OnGUI(Rect position)
        {
            GUILayout.BeginArea(position);

            GUILayout.Space(2);
            GUILayout.BeginHorizontal(EditorStyles.inspectorFullWidthMargins);

            var content = new GUIContent(EditorGUIUtility.IconContent("tab_prev").image, "Back");
            if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20f), GUILayout.Height(20f)))
            {
                GoBack();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Propertyes", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();

            // Draw header
            content = new GUIContent(EditorGUIUtility.IconContent("_Help").image, "Help");
            if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20f), GUILayout.Height(20f)))
            {
                Application.OpenURL(helpURL);
            }

            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, EditorStyles.inspectorFullWidthMargins);

            var horizontal = position.width > 370;
            if (horizontal) { GUILayout.BeginHorizontal(); }

            // Show Languages ReorderableList
            GUILayout.BeginVertical();
            languagesList.DoLayoutList();
            GUILayout.EndVertical();
            GUILayout.Space(2);
            // Show supported types list
            GUILayout.BeginVertical();
            typesList.DoLayoutList();
            GUILayout.EndVertical();

            if (horizontal) { GUILayout.EndHorizontal(); }
            GUILayout.EndScrollView();

            GUILayout.EndArea();

            HandleKeyboard(Event.current);
        }

        /// <summary>
        /// Method called when the Back button is clicked
        /// </summary>
        public void GoBack()
        {
            languagesList.index = typesList.index = -1;
            OnBackButton?.Invoke();
            EditorGUI.FocusTextInControl(null);
        }

        /// <summary>
        /// Handles keystrokes
        /// </summary>
        /// <param name="curentEvent"></param>
        private void HandleKeyboard(Event curentEvent)
        {
            if (curentEvent.type == EventType.KeyDown)
            {
                switch (curentEvent.keyCode)
                {
                    case KeyCode.LeftArrow:
                        {
                            GoBack();
                            curentEvent.Use();
                        }
                        return;
                }
            }
        }
    }
}
