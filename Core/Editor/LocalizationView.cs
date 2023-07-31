using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	public class LocalizationView : IView
	{
		private object data;
		private LocalizationStorage storage;
		private Vector2 scrollPosition = Vector2.zero;
		private NoticeView noticeView;
		private bool editable = false;

		private LocalizationStorage LocalizationStorage { get => storage; }
		public object Data { 
			get => data;
			set {
				if(data != value)
				{
					data = value;
					scrollPosition = Vector2.zero;
					editable = false;
				}
			}
		}

		public LocalizationView(LocalizationStorage storage, NoticeView noticeView)
		{
			this.storage = storage;
			this.noticeView = noticeView;
		}

		public void OnGUI(Rect position)
		{
			if (Data is LocalizationTag tag)
			{
				var changeCheck = LocalizationStorage.ContainsLocalizationTag(tag);
				try
				{
					GUILayout.BeginArea(position);
					if (changeCheck) { EditorGUI.BeginChangeCheck(); }
					DrawHeader(position, tag);

					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
					GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);

					EditorGUI.BeginDisabledGroup(tag.IsDefault && !editable);
					DrawResources(tag, LocalizationManager.Languages);
					EditorGUI.EndDisabledGroup();

					GUILayout.EndVertical();
					GUILayout.EndScrollView();

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

		public static void DrawResources(LocalizationTag tag, Language[] languages, params GUILayoutOption[] options)
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

		private void DrawHeader(Rect position, LocalizationTag localization)
		{
			GUILayout.Space(2);
			GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
			GUILayout.Label("Name");
			GUILayout.FlexibleSpace();
			EditorGUI.BeginDisabledGroup(localization.IsDefault);
			localization.Name = GUILayout.TextField(localization.Name);
			EditorGUI.EndDisabledGroup();
			GUILayout.FlexibleSpace();

			var rect = new Rect(position);
			if (localization.IsDefault)
			{
				var content = new GUIContent(EditorGUIUtility.IconContent("CustomTool@2x").image, "Edit");
				editable = GUILayout.Toggle(editable, content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20));

			}
			else if (LocalizationStorage.ContainsLocalizationTag(localization))
			{
				var content = new GUIContent(EditorGUIUtility.IconContent("winbtn_win_close@2x").image, "Delete");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)))
				{
					LocalizationStorage.RemoveLocalizationTag(localization);
					noticeView.Show(rect, new GUIContent($"{localization.Name} has been deleted"));
					EditorGUI.FocusTextInControl(null);
				}
			}
			else
			{
				var content = new GUIContent(EditorGUIUtility.IconContent("CreateAddNew@2x").image, "Add");
				if (GUILayout.Button(content, EditorStyles.label, GUILayout.Width(20), GUILayout.Height(20)))
				{
					LocalizationStorage.AddLocalizationTag(localization);
					noticeView.Show(rect, new GUIContent($"{localization.Name} has been added"));
					EditorGUI.FocusTextInControl(null);
				}
			}
			GUILayout.EndHorizontal();
			ExtendedEditor.DrawLine(Color.black);
		}
	}
}