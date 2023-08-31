using UnityEditor;

[CustomEditor(typeof(LocalizationExample))]
public class LocalizationExampleEditor : Editor
{
    // Start is called before the first frame update
    private void OnEnable() => ((LocalizationExample)target).OnEnable();
}
