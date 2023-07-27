using UnityEditor;
using UnityEngine;

public class AssetCreator<T> : ClassCreator where T : ScriptableObject
{
    private string path;

    public AssetCreator(string path)
    {
        this.path = path ?? throw new System.ArgumentNullException(nameof(path));
    }

    public override void CreateClass()
    {
        var objectName = typeof(T).Name + ".asset";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }

        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path + objectName);
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
        Debug.Log($"{objectName} was created at the {path}");
    }
}
