using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	[CustomEditor(typeof(LocalizationStorage))]
	public class LocalizationStorageEditor : Editor
	{
		private LocalizationStorage storage;
		private Language[] languages;
		private Localization[] localizations;
		private bool[] foldouts;

		public void OnEnable()
		{
			storage = this.target as LocalizationStorage;
			languages = storage.Languages;
			localizations = storage.Localizations;
			foldouts = new bool[localizations.Length];
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			DrawResources();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open on window", GUILayout.Width(240f), GUILayout.Height(24f)))
			{
				LocalizationStorageWindow window = (LocalizationStorageWindow)EditorWindow.GetWindow(typeof(LocalizationStorageWindow));
				window.localizationStorage = storage;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		private void DrawResources()
		{
			for (int i = 0; i < localizations.Length; i++)
			{
				if(foldouts[i] = EditorGUILayout.Foldout(foldouts[i], localizations[i].Name))
				{
					for (int j = 0; j < languages.Length; j++)
					{
						DrawResource(languages[j].Name, localizations[i].Resources[j]);
					}
				}
			}
		}

		public static void DrawResource(string language, Resource resource)
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