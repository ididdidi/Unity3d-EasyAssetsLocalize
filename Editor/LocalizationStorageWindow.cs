using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
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

        public static void DrawResources(string language, Resource resource)
        {
			if (typeof(string).IsAssignableFrom(resource.Type))
			{
				EditorGUILayout.LabelField(language);
				resource.Data = EditorGUILayout.TextArea((string)resource.Data, GUILayout.Height(50f));
			}
			else
			{
				resource.Data = EditorGUILayout.ObjectField(language, (Object)resource.Data, resource.Type, false);
			}
		}
    }
}