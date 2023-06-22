﻿using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	/// <summary>
	/// Responsible for presenting the Localization Storage in the inspector.
	/// </summary>
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationStorageEditor : Editor
	{
		private LocalizationStorage storage;
		private int storageVersion;

		private bool foldout;
		private Language[] languages;
		private LocalizationTag[] localizationTags;
		private bool[] foldoutItems;

		/// <summary>
		/// Storage link caching.
		/// </summary>
		public void OnEnable() => storage = this.target as LocalizationStorage;

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			UpdateLocalizations();
			DrowLocalizations();
			DrowButton();
		}

		/// <summary>
		/// Updating LocalizationStorage data in the inspector window.
		/// </summary>
		private void UpdateLocalizations()
		{
			if (localizationTags == null || storageVersion != storage.Version)
			{
				languages = storage.Languages;
				localizationTags = storage.LocalizationTags;
				foldoutItems = new bool[localizationTags.Length];
				storageVersion = storage.Version;
			}
		}

		/// <summary>
		/// Button to display storage resources in a separate window.
		/// </summary>
		private void DrowButton()
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Edit in window", GUILayout.Width(240f), GUILayout.Height(24f)))
			{
				LocalizationStorageWindow window = (LocalizationStorageWindow)EditorWindow.GetWindow(typeof(LocalizationStorageWindow), false, storage.name);
				window.LocalizationStorage = storage;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Simple display of localization resources.
		/// </summary>
		private void DrowLocalizations()
		{
			EditorGUI.indentLevel++;
			if (foldout = EditorGUILayout.Foldout(foldout, "Localization resources"))
			{
				EditorGUI.indentLevel++;
				for (int i = 0; i < localizationTags.Length; i++)
				{
					if (foldoutItems[i] = EditorGUILayout.Foldout(foldoutItems[i], localizationTags[i].Name))
					{
					//	LocalizationResourceView.DisplayResources(localizationTags[i], languages);
					}
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}
	}
}