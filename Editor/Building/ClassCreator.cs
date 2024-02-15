namespace EasyAssetsLocalize
{
    /// <summary>
    /// Helper class for code generation of other classes.
    /// </summary>
    internal static class ClassCreator
    {
        /// <summary>
        /// Method for creating a class file.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="path">Directory path</param>
        /// <param name="code">Code</param>
        /// <param name="comment">Comment</param>
        public static void CreateClass(string className, string path, string code, string comment)
        {
            string fullName = $"{path}{className}.cs";
            if (!System.IO.File.Exists(fullName)) { System.IO.File.WriteAllText(fullName, comment + code); }
        }
    }
}