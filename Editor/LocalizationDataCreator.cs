using UnityEditor;
using UnityEngine;

namespace Localization
{
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
                var asset = ScriptableObject.CreateInstance<LocalizationStorage>();

                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;
            }
        }
    }
}
