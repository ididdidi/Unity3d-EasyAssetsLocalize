using UnityEditor;
using UnityEngine;
using EasyAssetsLocalize;

namespace EasyAssetsLocalize
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
        private int StorageVersion { get; set; }
        private SearchDropDownWindow DropDownWindow { get; set; }

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
            /// When adding a component to the scene
            if (string.IsNullOrEmpty(Component.ID)) { 
                var defaultLocal = Storage.GetDefaultLocalization(Component.Type);
                Localization = defaultLocal;
                Component.ID = defaultLocal.ID;
                return;
            }
        }

        /// <summary>
        /// Standard Method for displaying fields in an Inspector Window.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (StorageVersion != Storage.Version || StorageVersion == 0)
            {
                Localization = Component.GetLocalization();
                StorageVersion = Storage.Version;
            }

            DrawDefaultInspector();
            DrawLocalization();
            DrawHandler();
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
                LocalizationView.DrawResources(Storage, Localization, LocalizationManager.Languages, GUILayout.Height(50f));
                EditorGUI.EndDisabledGroup();
                if (EditorGUI.EndChangeCheck()) { LocalizationManager.Storage?.ChangeVersion(); }
            }

            if (EditorExtends.CenterButton(Localization == null ? "Set Localization" : "Change Localization")) { ShowSearchWindow(); }
        }

        /// <summary>
        /// Opens a dropdown window with a list of localizations.
        /// </summary>
        private void ShowSearchWindow()
        {
            DropDownWindow = SearchDropDownWindow.Show(new LocalizationSearchProvider(Storage, Component.Type), SetLocaloization);
        }

        /// <summary>
        /// Method for handling localization selection from the list.
        /// </summary>
        /// <param name="data">Localization instance as <see cref="object"/></param>
        /// <returns>Close window</returns>
        private void SetLocaloization(object data)
        {
            if (data is Localization localization)
            {
                if (!Storage.ContainsLocalization(localization))
                {
                    Storage.AddLocalization(localization);
                }
                Component.ID = localization.ID;
                Localization = Component.GetLocalization();
                EditorUtility.SetDirty(Component);
                DropDownWindow?.Close();
            }
        }
    }
}