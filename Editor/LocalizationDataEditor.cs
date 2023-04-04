using UnityEditor;
using UnityEngine;

namespace Localization
{
	[CustomEditor(typeof(LocalizationData))]
	public class LocalizationDataEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Edit localization"))
			{
				LocalizationEditorWindow window = (LocalizationEditorWindow)EditorWindow.GetWindow(typeof(LocalizationEditorWindow));
				window.localization = this.target as LocalizationData;
				window.minSize = new Vector2(640, 240);
			}
		}
	}
}