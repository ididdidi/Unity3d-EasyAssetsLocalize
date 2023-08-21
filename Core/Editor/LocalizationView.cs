using UnityEditor;
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
		private NoticeView noticeView;

		private LocalizationStorage Storage { get => storage; }
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

		public LocalizationView(LocalizationStorage storage, NoticeView noticeView)
		{
			this.storage = storage ?? throw new System.ArgumentNullException(nameof(storage));
			this.noticeView = noticeView ?? throw new System.ArgumentNullException(nameof(noticeView));
		}

		public void OnGUI(Rect position)
		{
			if (Data is Localization localization)
			{
				var changeCheck = Storage.ContainsLocalization(localization);

				GUILayout.BeginArea(position);
				GUILayout.BeginVertical();
				GUILayout.Space(4);
				if (changeCheck) { EditorGUI.BeginChangeCheck(); }
				DrawHeader(position, localization);
				
				scrollPosition = GUILayout.BeginScrollView(scrollPosition);
				EditorGUI.BeginDisabledGroup(localization.IsDefault && !editable);
				DrawResources(Storage, localization, LocalizationManager.Languages);
				EditorGUI.EndDisabledGroup();
				GUILayout.EndScrollView();
				
				if (changeCheck && EditorGUI.EndChangeCheck())
				{
					Storage.ChangeVersion();
					EditorUtility.SetDirty(Storage);
				}
				GUILayout.EndVertical();
				GUILayout.EndArea();
				
				HandleKeyboard(Event.current);
			}
		}

		private void DrawHeader(Rect position, Localization localization)
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
			EditorGUI.BeginChangeCheck();
			var name = GUILayout.TextField(localization.Name);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(Storage, $"Changed name from {localization.Name} to {name}");
				localization.Name = name;
			}
			EditorGUI.EndDisabledGroup();

			GUILayout.FlexibleSpace();
		
			if (localization.IsDefault)
			{
				content = new GUIContent(
					EditorGUIUtility.IconContent(editable ? "AssemblyLock" : "CustomTool").image,
					editable ? "Lock" : "Edit");
				editable = GUILayout.Toggle(editable, content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20));
		
			}
			else if (Storage.ContainsLocalization(localization))
			{
				content = new GUIContent(EditorGUIUtility.IconContent("winbtn_win_close").image, "Delete");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)) && ExtendedEditor.DeleteConfirmation(localization.Name))
				{
					Storage.RemoveLocalization(localization);
					noticeView.Show(position, new GUIContent($"{localization.Name} has been deleted"));
					GoBack();
				}
			}
			else
			{
				content = new GUIContent(EditorGUIUtility.IconContent("CreateAddNew").image, "Add");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)))
				{
					Storage.AddLocalization(localization);
					noticeView.Show(position, new GUIContent($"{localization.Name} has been added"));
					GoBack();
				}
			}

			GUILayout.EndHorizontal();
			ExtendedEditor.DrawLine(Color.black);
		}

		public static void DrawResources(LocalizationStorage storage, Localization localization, Language[] languages, params GUILayoutOption[] options)
		{
			GUIStyle style = new GUIStyle(EditorStyles.textArea);
			style.wordWrap = true;
			var isString = typeof(string).IsAssignableFrom(localization.Type);

			for (int i = 0; i < localization.Resources.Count; i++)
			{
				object tmpData = localization.Resources[i].Data;
				EditorGUI.BeginChangeCheck();

				if (isString)
				{
					EditorGUILayout.LabelField(languages[i].ToString());
					tmpData = EditorGUILayout.TextArea((string)tmpData, style, options);
				}
				else
				{
					tmpData = EditorGUILayout.ObjectField(languages[i].ToString(), (Object)tmpData, localization.Type, false, options);
				}

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(storage, $"Changed data for {localization.Name}");
					localization.Resources[i].Data = tmpData;
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
			if (curentEvent.type == EventType.KeyDown && curentEvent.keyCode == KeyCode.LeftArrow)
			{
				GoBack();
				curentEvent.Use();
			}
		}
	}
}