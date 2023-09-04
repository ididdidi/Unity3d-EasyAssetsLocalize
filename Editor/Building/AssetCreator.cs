using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class to create ScriptableObjects.
    /// </summary>
    internal static class AssetCreator
    {
        /// <summary>
        /// Method to create ScriptableObjects
        /// </summary>
        /// <typeparam name="T">Type extended from ScriptableObject</typeparam>
        /// <param name="path">Path to where the file is stored in the project</param>
        /// <param name="name">Name of file without extension</param>
        /// <returns>T-type instance</returns>
        public static T Create<T>(string path, string name = null) where T : ScriptableObject
        {
            var objectName = (string.IsNullOrEmpty(name) ? typeof(T).Name : name) + ".asset";
            if (!System.IO.Directory.Exists(path)) { System.IO.Directory.CreateDirectory(path); }

            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path + objectName);
            AssetDatabase.SaveAssets();

            Selection.activeObject = asset;
            Debug.Log($"{objectName} was created at the {path}");
            return asset;
        }
    }
}
