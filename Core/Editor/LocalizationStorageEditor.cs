using UnityEditor;
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

		private Language[] languages;
		private Localization[] localizations;
		private bool[] foldouts;

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
			if (localizations == null || storageVersion != storage.Version)
			{
				languages = storage.Languages;
				localizations = storage.Localizations;
				foldouts = new bool[localizations.Length];
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
				window.localizationStorage = storage;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Simple display of localization resources.
		/// </summary>
		private void DrowLocalizations()
		{
			var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
			EditorGUILayout.LabelField("Localization resources", style, GUILayout.ExpandWidth(true));

			for(int i=0; i < localizations.Length; i++)
			{
				if (foldouts[i] = EditorGUILayout.Foldout(foldouts[i], localizations[i].Name))
				{
					LocalizationResourceView.DisplayResources(localizations[i], languages);
				}
			}
		}
	}
}