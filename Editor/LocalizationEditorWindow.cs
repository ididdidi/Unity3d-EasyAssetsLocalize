using UnityEditor;
using UnityEngine;

namespace Localization
{
	public class LocalizationEditorWindow : EditorWindow
	{
		public LocalizationStorage localization;
		public SerializedObject serializedObject;
		private LocalizationReorderableList localizationsList;

		void OnGUI()
		{
			if (localizationsList == null && localization) { localizationsList = new LocalizationReorderableList(localization); }
			localizationsList?.DoLayoutList();

			if (GUI.changed)
			{
				EditorUtility.SetDirty(localization);
			}
		}
	}
}