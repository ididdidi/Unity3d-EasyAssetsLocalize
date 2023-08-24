public static class ClassCreator
{
    /// <summary>
    /// Method for creating a class file.
    /// </summary>
    /// <param name="className">Class name</param>
    /// <param name="path">Directory path</param>
    /// <param name="code">Code</param>
    /// <param name="comment">Comment</param>
    public static void CreateClass(string className, string path, string code, string comment = "// Class generated automatically")
    {
        System.IO.File.WriteAllText($"{path}{className}.cs", comment + code);
    }
}
