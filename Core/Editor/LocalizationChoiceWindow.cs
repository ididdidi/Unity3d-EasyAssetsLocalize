using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationChoiceWindow : EditorWindow
	{
		private readonly Vector2 size = new Vector2(240f, 320f);
		public LocalizationStorage LocalizationStorage { get; set; }

		private Vector2 scrollPosition = Vector2.zero;
		private Localization[] localizations;
		private List<Localization> selected = new List<Localization>();
		private LocalizationSearch search;

		public void OnEnable()
		{
			minSize = size;
		}

		private void OnGUI()
		{
			Initiate();
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			DrawSearchField();
			DrawLocalizationsList();

			GUILayout.BeginHorizontal();
			CreateButton();
			CancelButton();
			GUILayout.EndHorizontal();

			EditorGUILayout.EndScrollView();
		}

		private void Initiate()
		{
			if (search == null && LocalizationStorage)
			{
				search = new LocalizationSearch(LocalizationStorage);
				localizations = search.GetResult();
			}
		}

		/// <summary>
		/// Display a field for searching by tags.
		/// </summary>
		private void DrawSearchField()
		{
			if (search != null && search.SearchFieldChanged())
			{
				localizations = search.GetResult();
			}
		}

		private void DrawLocalizationsList()
		{
			if (localizations != null)
			{
				for (int i=0; i < localizations.Length; i++)
				{
					var isSelected = selected.Contains(localizations[i]);
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(localizations[i].Name);
					if (isSelected != EditorGUILayout.Toggle(isSelected))
					{
						if (isSelected) { selected.Remove(localizations[i]); }
						else { selected.Add(localizations[i]); }
					}
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		private void CreateButton()
		{
			GUI.enabled = selected?.Count > 0;
			if (GUILayout.Button("Confirm choice"))
			{
				this.Close();
			}
			GUI.enabled = true;
		}

		private void CancelButton()
		{
			if (GUILayout.Button("Cancel")) { this.Close(); }
		}
	}
}