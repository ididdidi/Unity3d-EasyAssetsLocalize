﻿using System.Collections.Generic;
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
		private LocalizationTagCreateWindow tagCreateWindow;


		public void OnEnable()
		{
			minSize = size;
			maxSize = size;
		}

		public static SetLocalizationTagWindow GetInstance(LocalizationController controller, string tagName = "")
		{
			var window = (SetLocalizationTagWindow)EditorWindow.GetWindow(typeof(SetLocalizationTagWindow), true, tagName);
			window.LocalizationController = controller;
			return window;
		}

		private void OnGUI()
		{
			GUILayout.BeginHorizontal();
			AddNewTagButton();
			DrawSearchField();
			GUILayout.EndHorizontal();

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			DrawLocalizationsList();
			EditorGUILayout.EndScrollView();

			GUILayout.BeginHorizontal();
			CancelButton();
			SetLocalizationTagsButton();
			GUILayout.EndHorizontal();
		}

		/// <summary>
		/// Display a field for searching by tags.
		/// </summary>
		private void DrawSearchField()
		{
			if (search == null && LocalizationController)
			{
				search = new LocalizationSearch(LocalizationController.LocalizationStorage);
				localizations = search.GetResult();
			}

			if (search != null && search.IsChanged())
			{
				localizations = search.GetResult();
			}
		}

		/// <summary>
		/// Display a button to add a new language.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/></param>
		/// <param name="dX">Offset X</param>
		private void AddNewTagButton()
		{
			GUIContent icon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add Localization Tag");
			GUIStyle style = "RL FooterButton";
			style.margin.top = 2;
			style.margin.left = 2;
			if (GUILayout.Button(icon, style)) { CreateNewTag(); }
		}

		private void CreateNewTag()
		{
			tagCreateWindow = LocalizationTagCreateWindow.GetInstance(
				(tag) => { 
					LocalizationController.LocalizationStorage.AddLocalizationTag(tag); 
					selected.Add(tag);
					search.Key = tag.Name;
				}, search.Key);
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
			Undo.RecordObject(LocalizationController, LocalizationController.name);
			for (int i = 0; i < selected.Count; i++)
			{
				//System.Type arg = selected[i].Resources[0].Data.GetType();
				//System.Type genericClass = typeof(LocalizationReceiver<>);
				//System.Type createdClass = genericClass.MakeGenericType(arg);
				//
				//// Result is of TypeReporter<T> type. Invoke desired methods.
				//var result = (LocalizationReceiver)System.Activator.CreateInstance(createdClass, selected[i]);
				//Debug.Log(result.Name);

				if(selected[i].Resources[0].Data is Sprite)
				{
					LocalizationController?.AddLocalizationReceiver(new SpriteReceiver(selected[i]));
				}
			}
			EditorUtility.SetDirty(LocalizationController);
			EditorSceneManager.MarkSceneDirty(LocalizationController.gameObject.scene);
			this.Close();
		}

		private void SetLocalizationTagsButton()
		{
			EditorGUI.BeginDisabledGroup(selected?.Count < 1);
			if (GUILayout.Button("Confirm choice"))
			{
				SetLocalizationTags();
			}
			EditorGUI.EndDisabledGroup();
		}

		private void CancelButton()
		{
			if (GUILayout.Button("Cancel")) { this.Close(); }
		}

		private void OnDestroy() => tagCreateWindow?.Close();
	}
}