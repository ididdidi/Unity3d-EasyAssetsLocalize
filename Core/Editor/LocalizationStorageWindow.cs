using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow
	{
		private LocalizationStorage localizationStorage;
		private ReorderableLocalizationList localizationsList;
		private Vector2 scrollPosition = Vector2.zero;

		private Vector2 Size { set => this.minSize = this.maxSize = value; }

		public LocalizationStorage LocalizationStorage { get => localizationStorage; set => CreateLocalizationList(localizationStorage = value); }
		
		private void CreateLocalizationList(LocalizationStorage localizationStorage)
		{
			localizationsList = new ReorderableLocalizationList(localizationStorage);
			Size = localizationsList.GetSize();
		}

		void OnGUI()
		{
			if (localizationsList == null && localizationStorage) {
				localizationsList = new ReorderableLocalizationList(localizationStorage);
				Size = localizationsList.GetSize();
			}

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			localizationsList?.DoLayoutList();
			EditorGUILayout.EndScrollView();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localizationStorage);
				Size = localizationsList.GetSize();
			}
		}
    }
}