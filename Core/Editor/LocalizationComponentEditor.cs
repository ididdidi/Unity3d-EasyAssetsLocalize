using UnityEditor;
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
        private Localization Localization { get; set; }
        private LocalizationStorage Storage { get => LocalizationManager.Storage; }
        private SearchDropDownWindow dropDownWindow;

        private SerializedProperty handler;
        /// <summary>
        /// Method for displaying a list of initialization handlers and changing localization.
        /// </summary>
        private void DrawHandler()
        {
            serializedObject.Update();
            if (Localization != null)
            {
                if (handler == null) { handler = serializedObject.FindProperty("handler"); }
                if (handler != null) { EditorGUILayout.PropertyField(handler); }
            }
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Standard method for initialization
        /// </summary>
        private void OnEnable()
        {
            Component = target as LocalizationComponent;
            SetLocalization(Component.ID);
        }

        /// <summary>
        /// Standard Method for displaying fields in an Inspector Window.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawLocalization();
            DrawHandler();
        }

        /// <summary>
        /// Initializes Localization in an editor instance.
        /// </summary>
        /// <param name="id">Localization identifier</param>
        private void SetLocalization(string id)
        {
            if (!string.IsNullOrEmpty(id)) { Localization = Storage.GetLocalization(id); }
        }

        /// <summary>
        /// Drawing localization fields in the inspector window.
        /// </summary>
        private void DrawLocalization()
        {
            if (Localization != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.BeginDisabledGroup(Localization.IsDefault);
                Localization.Name = EditorGUILayout.TextField("Localization name", Localization.Name);
                LocalizationView.DrawResources(Localization, LocalizationManager.Languages, GUILayout.Height(50f));
                EditorGUI.EndDisabledGroup();
                if (EditorGUI.EndChangeCheck()) { LocalizationManager.Storage?.ChangeVersion(); }
            }

            if (ExtendedEditor.CenterButton(Localization == null ? "Set Localization" : "Change Localization")) { ShowSearchWindow(); }
        }

        /// <summary>
        /// Opens a dropdown window with a list of localizations.
        /// </summary>
        private void ShowSearchWindow()
        {
            dropDownWindow = SearchDropDownWindow.Show(new LocalizationSearchProvider(Storage, SetLocaloization, null, Component.Type));
        }

        /// <summary>
        /// Method for handling localization selection from the list.
        /// </summary>
        /// <param name="data">Localization instance as <see cref="object"/></param>
        /// <returns>Close window</returns>
        private bool SetLocaloization(object data)
        {
            if (data is Localization localization)
            {
                if (!Storage.ContainsLocalization(localization))
                {
                    Storage.AddLocalization(localization);
                }
                Component.ID = localization.ID;
                SetLocalization(localization.ID);
                EditorUtility.SetDirty(Component);
                dropDownWindow?.Close();
                return true;
            }
            else return false;
        }
    }
}