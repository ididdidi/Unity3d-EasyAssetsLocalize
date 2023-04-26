using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationStorageEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Edit localization", GUILayout.Width(240f), GUILayout.Height(24f)))
			{
				LocalizationStorageWindow window = (LocalizationStorageWindow)EditorWindow.GetWindow(typeof(LocalizationStorageWindow));
				window.localizationStorage = this.target as LocalizationStorage;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
}