using UnityEditor;
using UnityEngine;

namespace UnityExtended
{
    /// <summary>
    /// Class for extending the capabilities of the Unity3d editor
    /// </summary>
    public abstract partial class ExtendedEditor : Editor
    {
        /// <summary>
        /// Method for finding a file in the Assets directory.
        /// </summary>
        /// <param name="fileName">File fullname</param>
        /// <returns>Path to the first file found, or <see cref="null"/></returns>
        public static string GetDirectory(string fileName)
        {
            string[] res = System.IO.Directory.GetFiles(Application.dataPath, fileName, System.IO.SearchOption.AllDirectories);
            if (res.Length == 0)
            {
                return null;
            }
            string path = res[0].Replace(fileName, "").Replace("\\", "/");
            return path;
        }

        /// <summary>
        /// Method for creating a class file.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="path">Directory path</param>
        /// <param name="code">Code</param>
        /// <param name="comment">Comment</param>
        public static void CreateClass(string className, string path, string code, string comment = "// Class generated automatically", bool overwrite = true)
        {
            if (overwrite || string.IsNullOrEmpty(GetDirectory($"{className}.cs")))
            {
                System.IO.File.WriteAllText($"{path}{className}.cs", comment + code);
            }
        }
    }
}