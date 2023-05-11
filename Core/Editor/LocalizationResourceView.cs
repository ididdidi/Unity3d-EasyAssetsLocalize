using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationResourceView
	{
		private LocalizationStorage storage;
		private int storageVersion;

		private Language[] languages;
		private LocalizationTag tag;
		private Localization localization;
		private bool foldout;

		public LocalizationResourceView(LocalizationStorage storage, LocalizationTag tag)
		{
			this.storage = storage;
			this.tag = tag;
		}

		private void UpdateLocalization()
		{
			if (localization == null || storageVersion != storage.Version)
			{
				localization = storage.GetLocalization(tag.ID);
				languages = storage.Languages;
				storageVersion = storage.Version;
			}
		}

		public void DrawResources()
		{
			UpdateLocalization();
			DrawResources(localization, languages);
		}

		public static void DrawResources(Localization localization, Language[] languages)
		{
			if (localization == null)
			{
				EditorGUILayout.HelpBox("Localization for this tag is not found! Add the tag to the localization controller for this scene.", MessageType.Error, true);
			}
			for (int i = 0; i < languages.Length; i++)
			{
				DrawResource(localization.Resources[i], languages[i].Name);
			}
		}

		private static void DrawResource(Resource resource, string language)
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