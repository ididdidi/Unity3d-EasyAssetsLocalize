using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Creates an instance of a script object of type LocalizationStorage
    /// </summary>
    public static class LocalizationBuilder
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            CreateLocalizationStorage();
        }

        public static void CreateLocalizationStorage()
        {
            if (!Resources.Load<LocalizationStorage>(nameof(LocalizationStorage)))
            {
                var path = GetDirectory($"{nameof(LocalizationStorage)}.cs")
                    .Replace("/Core", "/Resources")
                    .Replace(Application.dataPath, "Assets");

                new AssetCreator<LocalizationStorage>(path).CreateClass();

                var storage = AssetDatabase.LoadAssetAtPath<LocalizationStorage>($"{path}{nameof(LocalizationStorage)}.asset");
                storage.AddLanguage(new Language(Application.systemLanguage));
            }
        }

        public static void CreateComponent(object defaultValue)
        {
            if (defaultValue == null) { throw new System.ArgumentNullException(nameof(defaultValue)); }
            var path = GetDirectory($"{typeof(LocalizationComponentEditor).Name}.cs").Replace("/Core/Editor", "/Components");
            if (!System.IO.Directory.Exists($"{path}Editor/"))
            {
                System.IO.Directory.CreateDirectory($"{path}Editor/");
            }
            new LocalizationComponentCreator(path, defaultValue.GetType()).CreateClass();
            new LocalizationEditorCreator(path, defaultValue).CreateClass();
            AssetDatabase.Refresh();
        }

        public static void Remove(System.Type type)
        {
            var fileName = $"{type.Name}LocalizationEditor.cs";
            var path = GetDirectory(fileName);
            if (!string.IsNullOrEmpty(path))
            {
                System.IO.File.Delete($"{path}{fileName}");
                System.IO.File.Delete($"{path.Replace("/Editor", "")}{fileName.Replace("Editor", "")}");
                AssetDatabase.Refresh();
            }
            else throw new System.ArgumentNullException(fileName);
        }

        /// <summary>
        /// Method for finding a file in the Assets directory.
        /// </summary>
        /// <param name="fileName">File fullname</param>
        /// <returns>Path to the first file found, or <see cref="null"/></returns>
        public static string GetDirectory(string fileName)
        {
            string[] res = System.IO.Directory.GetFiles(Application.dataPath, fileName, System.IO.SearchOption.AllDirectories);
            if (res.Length == 0) { return null; }
            string path = res[0].Replace(fileName, "").Replace("\\", "/");
            return path;
        }
    }
}
