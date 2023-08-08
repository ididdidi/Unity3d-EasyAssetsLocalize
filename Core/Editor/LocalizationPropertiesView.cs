using UnityEditor;
using UnityEngine;

namespace SimpleLocalization
{
    /// <summary>
    /// Visualizes localization properties in the inspector window.
    /// </summary>
    public class LocalizationPropertiesView : IView
    {
        private static readonly string helpURL = "https://ididdidi.ru/";
        private GUIStyle style;
        private LanguagesListView languagesList;
        private TypesListView typesList;
        private Vector2 scrollPosition = default;
        private System.Action onCloseButton;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onCloseButton">Callback to close view</param>
        public LocalizationPropertiesView(System.Action onCloseButton)
        {
            this.onCloseButton = onCloseButton ?? throw new System.ArgumentNullException(nameof(onCloseButton));
            languagesList = new LanguagesListView(LocalizationManager.Storage);
            typesList = new TypesListView();
        }

        /// <summary>
        /// Method for draw localization properties in the inspector window
        /// </summary>
        /// <param name="position"><see cref="Rect"/> position</param>
        public void OnGUI(Rect position)
        {
            // Set style settings
            if (style == null) { style = new GUIStyle("AM MixerHeader"); }

            var content = GUIContent.none;
            GUI.Label(position, content, "grey_border");

            GUILayout.BeginArea(position);
            GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);

            // Draw header
            content = new GUIContent(EditorGUIUtility.IconContent("tab_prev@2x").image, "Back");
            if (GUILayout.Button(content, style, GUILayout.Width(20f), GUILayout.Height(20f))) {
                GoBack();
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label("Propertyes", style);
            GUILayout.FlexibleSpace();

            content = new GUIContent(EditorGUIUtility.IconContent("_Help@2x").image, "Help");
            if (GUILayout.Button(content, style, GUILayout.Width(20f), GUILayout.Height(20f)))
            {
                Application.OpenURL(helpURL);
            }
            GUILayout.EndHorizontal();

            // Draw properties
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, EditorStyles.inspectorDefaultMargins);
            GUILayout.BeginHorizontal();

            // Show Languages ReorderableList
            GUILayout.BeginVertical();
            languagesList.DoLayoutList();
            GUILayout.EndVertical();

            GUILayout.Space(4);

            // Show supported types list
            GUILayout.BeginVertical();
            typesList.DoLayoutList();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            var curentEvent = Event.current;
            if (curentEvent.type == EventType.KeyDown && curentEvent.keyCode == KeyCode.Escape)
            {
                GoBack();
                curentEvent.Use();
            }
        }

        /// <summary>
        /// Method called when the Back button is clicked
        /// </summary>
        public void GoBack()
        {
            languagesList.index = typesList.index  = -1;
            onCloseButton();
            EditorGUI.FocusTextInControl(null);
        }
    }
}