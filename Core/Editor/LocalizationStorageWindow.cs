using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow
	{
		public LocalizationStorage localizationStorage;
		private ReorderableLocalizationList localizationsList;
		private Vector2 scrollPosition = Vector2.zero;

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