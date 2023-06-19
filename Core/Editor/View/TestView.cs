using UnityEditor;

public class TestView : IEditorView
{
    public void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Test passed successfully");
    }
}
