using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	public class LocalizationView : IEditorView
	{
		private LocalizationTag currentTag;
		private LocalizationStorage LocalizationStorage { get; }
		public LocalizationTag Tag { get; set; }
		private Vector2 scrollPosition = Vector2.zero;

		public LocalizationView(LocalizationStorage storage)
		{
			LocalizationStorage = storage ?? throw new System.ArgumentNullException(nameof(storage));
		}

		public void OnGUI(IContext context)
		{
			GUI.Label(context.position, GUIContent.none, "grey_border");

			if (Tag != null)
			{
				if(currentTag != Tag)
				{
					currentTag = Tag;
					context.Repaint();
				}

				EditorGUI.BeginChangeCheck();
					GUILayout.BeginArea(context.position);
						GUILayout.Space(2);
						GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
							DrawLocalizationName(Tag);
						GUILayout.EndHorizontal();
						ExtendedEditorGUI.DrawLine(Color.black);
						scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
							GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
								DrawResources(Tag, LocalizationStorage.Languages);
							GUILayout.EndVertical();
						GUILayout.EndScrollView();
					GUILayout.EndArea();
				if (EditorGUI.EndChangeCheck()) { LocalizationStorage?.ChangeVersion(); }
			}
		}

		public static void DrawLocalizationName(LocalizationTag localization)
		{
			GUILayout.Label("Name");
				GUILayout.FlexibleSpace();
					localization.Name = GUILayout.TextField(localization.Name);
					ExtendedEditorGUI.GetLastControlId().ReleaseOnClick();
				GUILayout.FlexibleSpace();
			GUILayout.Button("Delete");
		}

		public static void DrawResources(LocalizationTag tag, Language[] languages, params GUILayoutOption[] options)
		{
			GUIStyle style = new GUIStyle(EditorStyles.textArea);
			style.wordWrap = true;
			var isString = tag.Type.IsAssignableFrom(typeof(string));
			for (int i = 0; i < tag.Resources.Count; i++)
			{
				if (isString)
				{
					EditorGUILayout.LabelField(languages[i].Name);
					tag.Resources[i].Data = EditorGUILayout.TextArea((string)tag.Resources[i].Data, style, options);
					ExtendedEditorGUI.GetLastControlId().ReleaseOnClick();
				}
				else
				{
					tag.Resources[i].Data = EditorGUILayout.ObjectField(languages[i].Name, (Object)tag.Resources[i].Data, tag.Type, false, options);
				}
			}
		}
	}
}