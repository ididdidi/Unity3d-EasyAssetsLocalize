using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ResourceLocalization
{
    public class SetLocalizationTagWindow : EditorWindow
	{
		private readonly Vector2 size = new Vector2(242f, 320f);
		public LocalizationController LocalizationController { get; set; }

		private Vector2 scrollPosition = Vector2.zero;
		private LocalizationTag[] localizations;
		private List<LocalizationTag> selected = new List<LocalizationTag>();
		private LocalizationSearch search;

		public void OnEnable()
		{
			minSize = size;
			maxSize = size;
		}

		private void OnGUI()
		{
			Initiate();
			DrawSearchField();

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			DrawLocalizationsList();
			EditorGUILayout.EndScrollView();

			GUILayout.BeginHorizontal();
			CancelButton();
			SetLocalizationTagsButton();
			GUILayout.EndHorizontal();
		}

		private void Initiate()
		{
			if (search == null && LocalizationController)
			{
				search = new LocalizationSearch(LocalizationController.LocalizationStorage);
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
					EditorGUILayout.LabelField($"{localizations[i].Name} ({localizations[i].Resources[0].Data.GetType().Name})");
					if (isSelected != EditorGUILayout.Toggle(isSelected))
					{
						if (isSelected) { selected.Remove(localizations[i]); }
						else { selected.Add(localizations[i]); }
					}
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		private void SetLocalizationTags()
		{
			for (int i = 0; i < selected.Count; i++)
			{
				LocalizationController.AddLocalizationReceiver(selected[i].CreateReceiver());
			}
		}

		private void SetLocalizationTagsButton()
		{
			EditorGUI.BeginDisabledGroup(selected?.Count < 1);
			if (GUILayout.Button("Confirm choice"))
			{
				Undo.RecordObject(LocalizationController, LocalizationController.name);
				SetLocalizationTags();
				EditorUtility.SetDirty(LocalizationController);
				EditorSceneManager.MarkSceneDirty(LocalizationController.gameObject.scene);
				this.Close();
			}
			EditorGUI.EndDisabledGroup();
		}

		private void CancelButton()
		{
			if (GUILayout.Button("Cancel")) { this.Close(); }
		}
	}
}