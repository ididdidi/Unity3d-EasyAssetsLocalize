﻿using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationStorageEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Edit localization"))
			{
				LocalizationEditorWindow window = (LocalizationEditorWindow)EditorWindow.GetWindow(typeof(LocalizationEditorWindow));
				window.localizationStorage = this.target as LocalizationStorage;
			}
		}
	}
}