using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationEditorWindow : EditorWindow
	{
		public LocalizationStorage localizationStorage;
		private ReorderableLocalizationList localizationsList;

		void OnGUI()
		{
			if (localizationsList == null && localizationStorage) { localizationsList = new ReorderableLocalizationList(localizationStorage); }
			localizationsList?.DoLayoutList();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localizationStorage);
			}
		}
	}
}