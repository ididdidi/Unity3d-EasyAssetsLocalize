using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationEditorWindow : EditorWindow
	{
		public LocalizationStorage localizationStorage;
		private LocalizationReorderableList localizationsList;

		void OnGUI()
		{
			if (localizationsList == null && localizationStorage) { localizationsList = new LocalizationReorderableList(localizationStorage); }
			localizationsList?.DoLayoutList();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localizationStorage);
			}
		}
	}
}