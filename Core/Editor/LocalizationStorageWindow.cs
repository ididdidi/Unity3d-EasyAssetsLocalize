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

		public LocalizationStorage LocalizationStorage { get => localizationStorage; set => CreateLocalizationList(localizationStorage = value); }
		
		private void CreateLocalizationList(LocalizationStorage localizationStorage)
		{
			localizationsList = new ReorderableLocalizationList(localizationStorage);
			this.minSize = localizationsList.GetSize();
		}

		void OnGUI()
		{
			if (localizationsList == null && localizationStorage) {
				localizationsList = new ReorderableLocalizationList(localizationStorage);
				this.minSize = localizationsList.GetSize();
			}

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			localizationsList?.DoLayoutList();
			EditorGUILayout.EndScrollView();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localizationStorage);
				this.minSize = localizationsList.GetSize();
			}
		}
    }
}