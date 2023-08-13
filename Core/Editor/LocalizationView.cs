using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	public class LocalizationView : IView, IEditorView
	{
		private object data;
		private LocalizationStorage storage;
		private System.Action onBackButton;
		private Rect resourcesViewRect = Rect.zero;
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

		public LocalizationView(LocalizationStorage storage, System.Action onBackButton)
		{
			this.storage = storage ?? throw new System.ArgumentNullException(nameof(storage));
			this.onBackButton = onBackButton ?? throw new System.ArgumentNullException(nameof(onBackButton));
		}

		public void OnGUI(Rect position)
		{
			if (Data is Localization tag)
			{
				var changeCheck = LocalizationStorage.ContainsLocalization(tag);
				try
				{
					GUILayout.BeginArea(position);
					if (changeCheck) { EditorGUI.BeginChangeCheck(); }
					DrawHeader(tag);

					var rect = EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
					if(!rect.Equals(Rect.zero)) { resourcesViewRect = rect; }

					scrollPosition = GUILayout.BeginScrollView(scrollPosition);
					EditorGUI.BeginDisabledGroup(tag.IsDefault && !editable);
					DrawResources(tag, LocalizationManager.Languages);
					EditorGUI.EndDisabledGroup();
					GUILayout.EndScrollView();

					EditorGUILayout.EndVertical();

					if (changeCheck && EditorGUI.EndChangeCheck())
					{
						LocalizationStorage?.ChangeVersion();
						EditorUtility.SetDirty(LocalizationStorage);
					}
					GUILayout.EndArea();
				}
				catch (System.ArgumentException) { }
			}
		}

		private void DrawHeader(Localization localization)
		{
			GUILayout.Space(4);
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
			onBackButton();
			scrollPosition = Vector2.zero;
			EditorGUI.FocusTextInControl(null);
		}
	}
}