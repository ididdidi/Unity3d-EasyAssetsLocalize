using UnityEditor;
using UnityEngine;

public static class AssetCreator
{
    public static T Create<T>(string path, string name = null) where T : ScriptableObject
    {
        var objectName = (string.IsNullOrEmpty(name)? typeof(T).Name : name) + ".asset";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path + objectName);
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
        Debug.Log($"{objectName} was created at the {path}");
        return asset;
    }
}
