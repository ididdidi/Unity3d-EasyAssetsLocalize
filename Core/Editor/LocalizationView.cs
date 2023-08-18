﻿using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	public class LocalizationView : IEditorView
	{
		private object data;
		private LocalizationStorage storage;
		private Vector2 scrollPosition;
		private bool editable = false;

		private LocalizationStorage LocalizationStorage { get => storage; }
		public object Data { 
			get => data;
			set {
				if(data != value)
				{
					data = value;
					editable = false;
				}
			}
		}

		public System.Action OnBackButton { get; set; }

		public LocalizationView(LocalizationStorage storage)
		{
			this.storage = storage ?? throw new System.ArgumentNullException(nameof(storage));
		}

		public void OnGUI(Rect position)
		{
			if (Data is Localization localization)
			{
				var changeCheck = LocalizationStorage.ContainsLocalization(localization);

				GUILayout.BeginArea(position);
				GUILayout.BeginVertical();
				GUILayout.Space(4);
				if (changeCheck) { EditorGUI.BeginChangeCheck(); }
				DrawHeader(localization);
				
				scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				EditorGUI.BeginDisabledGroup(localization.IsDefault && !editable);
				DrawResources(localization, LocalizationManager.Languages);
				EditorGUI.EndDisabledGroup();
				GUILayout.EndScrollView();
				
				if (changeCheck && EditorGUI.EndChangeCheck())
				{
					LocalizationStorage?.ChangeVersion();
					EditorUtility.SetDirty(LocalizationStorage);
				}
				GUILayout.EndVertical();
				GUILayout.EndArea();
				
				HandleKeyboard(Event.current);
			}
		}

		private void DrawHeader(Localization localization)
		{
			GUILayout.BeginHorizontal(EditorStyles.inspectorFullWidthMargins);

			ExtendedEditor.BeginIgnoreChanges();
			var content = new GUIContent(EditorGUIUtility.IconContent("tab_prev").image, "Back");
			if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20f), GUILayout.Height(20f)))
			{
				GoBack();
			}
			ExtendedEditor.EndIgnoreChanges();
		
			GUILayout.FlexibleSpace();
			EditorGUI.BeginDisabledGroup(localization.IsDefault);
			localization.Name = GUILayout.TextField(localization.Name);
			EditorGUI.EndDisabledGroup();
			GUILayout.FlexibleSpace();
		
			if (localization.IsDefault)
			{
				content = new GUIContent(
					EditorGUIUtility.IconContent(editable ? "AssemblyLock" : "CustomTool").image,
					editable ? "Lock" : "Edit");
				editable = GUILayout.Toggle(editable, content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20));
		
			}
			else if (LocalizationStorage.ContainsLocalization(localization))
			{
				content = new GUIContent(EditorGUIUtility.IconContent("winbtn_win_close").image, "Delete");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)))
				{
					LocalizationStorage.RemoveLocalization(localization);
					GoBack();
				}
			}
			else
			{
				content = new GUIContent(EditorGUIUtility.IconContent("CreateAddNew").image, "Add");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)))
				{
					LocalizationStorage.AddLocalization(localization);
					GoBack();
				}
			}

			GUILayout.EndHorizontal();
			ExtendedEditor.DrawLine(Color.black);
		}

		public static void DrawResources(Localization tag, Language[] languages, params GUILayoutOption[] options)
		{
			GUIStyle style = new GUIStyle(EditorStyles.textArea);
			style.wordWrap = true;
			var isString = typeof(string).IsAssignableFrom(tag.Type);

			for (int i = 0; i < tag.Resources.Count; i++)
			{
				if (isString)
				{
					EditorGUILayout.LabelField(languages[i].ToString());
					tag.Resources[i].Data = EditorGUILayout.TextArea((string)tag.Resources[i].Data, style, options);
				}
				else
				{
					tag.Resources[i].Data = EditorGUILayout.ObjectField(languages[i].ToString(), (Object)tag.Resources[i].Data, tag.Type, false, options);
				}
			}
		}

		/// <summary>
		/// Method called when the Back button is clicked
		/// </summary>
		public void GoBack()
		{
			OnBackButton?.Invoke();
			scrollPosition = Vector2.zero;
			EditorGUI.FocusTextInControl(null);
		}

		/// <summary>
		/// Handles keystrokes
		/// </summary>
		/// <param name="curentEvent"></param>
		private void HandleKeyboard(Event curentEvent)
		{
			if (curentEvent.type == EventType.KeyDown)
			{
				switch (curentEvent.keyCode)
				{
					case KeyCode.LeftArrow:
						{
							GoBack();
							curentEvent.Use();
						}
						return;
				}
			}
		}
	}
}