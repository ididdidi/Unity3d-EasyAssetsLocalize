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
		private LocalizationReceiver receiver;
		private LocalizationTag localizationTag;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="storage">Repository with data about localization resources</param>
		/// <param name="tag">Localization resource tag</param>
		public LocalizationResourceView(LocalizationStorage storage, LocalizationReceiver receiver)
		{
			this.storage = storage;
			this.receiver = receiver;
		}

		/// <summary>
		/// Method for displaying localization data in the inspector
		/// </summary>
		public void Show()
		{
			UpdateLocalization();
			DisplayResources(localizationTag, languages);
		}

		/// <summary>
		/// Method for updating localization data.
		/// </summary>
		private void UpdateLocalization()
		{
			if (localizationTag == null || storageVersion != storage.Version)
			{
				localizationTag = storage.GetLocalizationTag(receiver.ID);
				languages = storage.Languages;
				storageVersion = storage.Version;
			}
		}

		/// <summary>
		/// Displays a list of localization resources.
		/// </summary>
		/// <param name="localization">Localization resources</param>
		/// <param name="languages">List of languages</param>
		public static void DisplayResources(LocalizationTag localizationTag, Language[] languages)
		{
			if (localizationTag == null)
			{
				EditorGUILayout.HelpBox("Localization for this tag is not found! Add the tag to the localization controller for this scene.", MessageType.Error, true);
			}
			for (int i = 0; i < languages.Length; i++)
			{
				localizationTag.Resources[i] = EditorGUILayout.ObjectField(languages[i].Name, localizationTag.Resources[i], localizationTag.Resources[i].GetType(), false);
			}
		}

	//	/// <summary>
	//	/// Displays localization resource
	//	/// </summary>
	//	/// <param name="resource">localization resource</param>
	//	/// <param name="language">language of the resource</param>
	//	private static void DrawResource(Object resource, string language)
	//	{
	//		resource = EditorGUILayout.ObjectField(language, resource, resource.GetType(), false);
	//	}
	}
}