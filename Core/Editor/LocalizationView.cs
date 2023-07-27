using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	public class LocalizationView
	{
		private LocalizationStorage LocalizationStorage { get; }
		public object Data { get; set; }
		private Vector2 scrollPosition = Vector2.zero;
		private NoticeView noticeView;

		public LocalizationView(LocalizationStorage storage, NoticeView noticeView)
		{
			LocalizationStorage = storage;
			this.noticeView = noticeView;

		}

		public void OnGUI(Rect position)
		{
			GUI.Label(position, GUIContent.none, "grey_border");

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
					DrawResources(tag, LocalizationManager.Languages);
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
			else if (Data is GUIContent content)
			{
				var rect = new Rect(0f, 0f, 128f, 128f);
				rect.center = position.center;
				rect.y -= 25f;
				GUI.DrawTexture(rect, content.image, ScaleMode.ScaleToFit);

				position.y += 55f;
				var style = new GUIStyle("AM MixerHeader");
				style.alignment = TextAnchor.MiddleCenter;
				style.fontSize = 32;
				GUI.Label(position, content.text, style);
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
			localization.Name = GUILayout.TextField(localization.Name);
			GUILayout.FlexibleSpace();

			var rect = new Rect(position);
			if (LocalizationStorage.ContainsLocalizationTag(localization))
			{
				if (GUILayout.Button("Delete")) { 
					LocalizationStorage.RemoveLocalizationTag(localization);
					noticeView.Show(rect, new GUIContent($"{localization.Name} has been deleted"));
					EditorGUI.FocusTextInControl(null);

				}
			}
			else
			{
				if (GUILayout.Button("Add")) { 
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