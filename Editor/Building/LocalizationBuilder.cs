using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Creates an instance of a script object of type LocalizationStorage.
    /// </summary>
    internal static class LocalizationBuilder
    {
        private static int count;
        public static readonly string localPath = "/Tools/EasyAssetsLocalize";

        /// <summary>
        /// Method for initializing storage and components required for localization.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            // Create repository if it doesn't exist
            var path = $"{Application.dataPath}{localPath}/Resources/LocalizationStorage.asset";
            if (!System.IO.File.Exists(path))
            {
                var storage = AssetCreator.Create<LocalizationStorage>($"Assets{localPath}/Resources/");
                storage.AddLanguage(new Language(Application.systemLanguage));
                CreateComponent(storage, "Text");
                Debug.Log("To get started with asset localization go to Window -> Localization Storage -> Settings(Gear) " +
                    "and add the default assets to the list of types.");
            }
        }

        /// <summary>
        /// Method for generating localization component code for a specific data type.
        /// </summary>
        /// <param name="storage"><see cref="Localization"/> data storage</param>
        /// <param name="defaultValue">Default localization data</param>
        public static void CreateComponent(IStorage storage, object defaultValue)
        {
            if (defaultValue == null) { throw new System.ArgumentNullException(nameof(defaultValue)); }

            var path = $"{Application.dataPath}{localPath}/Components/";
            if (!System.IO.Directory.Exists($"{path}Editor/"))
            {
                System.IO.Directory.CreateDirectory($"{path}Editor/");
            }

            var type = defaultValue.GetType();
            ClassCreator.CreateClass(type.Name + "Localization", path, new LocalizationComponentTemplate(type).Code);
            ClassCreator.CreateClass(type.Name + "LocalizationEditor", path + "Editor/", new LocalizationEditorTemplate(type).Code);

            var local = new Localization($"Default {defaultValue.GetType().Name} Localization", defaultValue, storage.Languages.Count, true);
            storage.AddLocalization(local);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Checks for the presence of a localization component for the specified resource type.
        /// </summary>
        /// <param name="type">resource type</param>
        /// <returns>Is there a component for this resource type</returns>
        public static bool Conteins(System.Type type) => !string.IsNullOrEmpty(GetDirectory($"{type.Name}LocalizationEditor.cs"));

        /// <summary>
        /// Delete localization component code for the specified resource type.
        /// </summary>
        /// <param name="type">resource type</param>
        public static void RemoveComponent(System.Type type)
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
            string[] res = System.IO.Directory.GetFiles(Application.dataPath + localPath, fileName, System.IO.SearchOption.AllDirectories);
            if (res.Length == 0) { return null; }
            string path = res[0].Replace(fileName, "").Replace("\\", "/");
            return path;
        }
    }
}
