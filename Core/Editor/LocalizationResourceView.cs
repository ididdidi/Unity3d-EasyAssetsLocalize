using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	/// <summary>
	/// Displays localization resources for a single tag.
	/// </summary>
	public class LocalizationResourceView
	{
		private LocalizationStorage storage;
		private int storageVersion;

		private Language[] languages;
		private LocalizationTag tag;
		private Localization localization;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="storage">Repository with data about localization resources</param>
		/// <param name="tag">Localization resource tag</param>
		public LocalizationResourceView(LocalizationStorage storage, LocalizationTag tag)
		{
			this.storage = storage;
			this.tag = tag;
		}

		/// <summary>
		/// Method for displaying localization data in the inspector
		/// </summary>
		public void Show()
		{
			UpdateLocalization();
			DisplayResources(localization, languages);
		}

		/// <summary>
		/// Method for updating localization data.
		/// </summary>
		private void UpdateLocalization()
		{
			if (localization == null || storageVersion != storage.Version)
			{
				localization = storage.GetLocalization(tag.ID);
				languages = storage.Languages;
				storageVersion = storage.Version;
			}
		}

		/// <summary>
		/// Displays a list of localization resources.
		/// </summary>
		/// <param name="localization">Localization resources</param>
		/// <param name="languages">List of languages</param>
		public static void DisplayResources(Localization localization, Language[] languages)
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

		/// <summary>
		/// Displays localization resource
		/// </summary>
		/// <param name="resource">localization resource</param>
		/// <param name="language">language of the resource</param>
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