using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	/// <summary>
	/// Responsible for presenting the Localization Storage in the inspector.
	/// </summary>
	[CustomEditor(typeof(LocalizationStorage))]
	public partial class LocalizationStorageEditor : Editor
	{
		private LocalizationStorage storage;
		private int storageVersion;

		private bool foldout;
		private Localization[] localizationTags;
		private bool[] foldoutItems;

		/// <summary>
		/// Storage link caching.
		/// </summary>
		public void OnEnable() => storage = this.target as LocalizationStorage;

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			UpdateLocalizations();
			DrowLocalizations();
			DrawButton();
		}

		/// <summary>
		/// Updating LocalizationStorage data in the inspector window.
		/// </summary>
		private void UpdateLocalizations()
		{
			if (localizationTags == null || storageVersion != storage.Version)
			{
				localizationTags = storage.Localizations;
				foldoutItems = new bool[localizationTags.Length];
				storageVersion = storage.Version;
			}
		}

		/// <summary>
		/// Button to display storage resources in a separate window.
		/// </summary>
		private void DrawButton()
		{
			if (ExtendedEditor.CenterButton("Edit in window"))
			{
				LocalizationStorageWindow.Show();
			}
		}

		/// <summary>
		/// Simple display of localization resources.
		/// </summary>
		private void DrowLocalizations()
		{
			EditorGUI.indentLevel++;
			if (foldout = EditorGUILayout.Foldout(foldout, "Localization resources"))
			{
				EditorGUI.indentLevel++;
				for (int i = 0; i < localizationTags.Length; i++)
				{
					if (foldoutItems[i] = EditorGUILayout.Foldout(foldoutItems[i], localizationTags[i].Name))
					{
						EditorGUI.BeginDisabledGroup(localizationTags[i].IsDefault);
						LocalizationView.DrawResources(localizationTags[i], LocalizationManager.Languages, GUILayout.Height(50f));
						EditorGUI.EndDisabledGroup();
					}
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}
	}
}