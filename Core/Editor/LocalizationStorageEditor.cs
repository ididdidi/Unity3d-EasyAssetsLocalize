using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationStorageEditor : Editor
	{
		private LocalizationStorage storage;
		private int storageVersion;

		private Language[] languages;
		private Localization[] localizations;
		private bool[] foldouts;

		public void OnEnable()
		{
			storage = this.target as LocalizationStorage;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			UpdateLocalizations();
			DrowLocalizations();
			DrowButton();
		}

		private void DrowButton()
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open on window", GUILayout.Width(240f), GUILayout.Height(24f)))
			{
				LocalizationStorageWindow window = (LocalizationStorageWindow)EditorWindow.GetWindow(typeof(LocalizationStorageWindow), false, storage.name);
				window.localizationStorage = storage;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void DrowLocalizations()
		{
			for(int i=0; i < localizations.Length; i++)
			{
				if (foldouts[i] = EditorGUILayout.Foldout(foldouts[i], localizations[i].Name))
				{
					LocalizationResourceView.DrawResources(localizations[i], languages);
				}
			}
		}

		private void UpdateLocalizations()
		{
			if (localizations == null || storageVersion != storage.Version)
			{
				languages = storage.Languages;
				localizations = storage.Localizations;
				foldouts = new bool[localizations.Length];
				storageVersion = storage.Version;
			}
		}
	}
}