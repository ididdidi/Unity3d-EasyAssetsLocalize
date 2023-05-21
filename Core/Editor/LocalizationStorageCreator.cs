using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Creates an instance of a script object of type LocalizationStorage
    /// </summary>
    public class LocalizationStorageCreator
    {
        [MenuItem("Assets/Create/Localization", false, 1)]
        public static void CreateLocalizationDataAsset()
        {
            var selectionPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(selectionPath))
            {
                selectionPath = Application.dataPath;
            }

            var path = EditorUtility.SaveFilePanelInProject(
                                             "Create Localization storage",
                                             "NewLocalizationData",
                                             "asset",
                                             string.Empty,
                                             selectionPath);

            if (path.Length > 0)
            {
                var storage = ScriptableObject.CreateInstance<LocalizationStorage>();
                storage.AddLanguage(new Language(Application.systemLanguage.ToString()));

                AssetDatabase.CreateAsset(storage, path);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = storage;
            }
        }
    }
}
