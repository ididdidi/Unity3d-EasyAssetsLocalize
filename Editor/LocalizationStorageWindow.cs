using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationStorageWindow : EditorWindow
	{
		public LocalizationStorage localizationStorage;
		private ReorderableLocalizationList localizationsList;

		void OnGUI()
		{
			if (localizationsList == null && localizationStorage) {
				localizationsList = new ReorderableLocalizationList(localizationStorage);
				this.minSize = localizationsList.GetSize();
			}

			localizationsList?.DoLayoutList();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localizationStorage);
				this.minSize = localizationsList.GetSize();
			}
		}
	}
}