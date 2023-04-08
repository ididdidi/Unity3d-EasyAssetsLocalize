using UnityEditor;
using UnityEngine;

namespace Localization
{
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationDataEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Edit localization"))
			{
				LocalizationEditorWindow window = (LocalizationEditorWindow)EditorWindow.GetWindow(typeof(LocalizationEditorWindow));
				window.localization = this.target as LocalizationStorage;
				window.serializedObject = this.serializedObject;
				window.minSize = new Vector2(640, 240);
			}
		}
	}
}