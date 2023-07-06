using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	public class LocalizationView
	{
		private readonly Color background = new Color(0.22f, 0.22f, 0.22f);
		private LocalizationStorage LocalizationStorage { get; }
		public LocalizationTag Tag { get; set; }
		private Vector2 scrollPosition = Vector2.zero;

		EditorWindow window;
		GUIContent noticeContent;
		long lastTime;
		bool notice;
		float curentNotice;

		public LocalizationView(EditorWindow window, LocalizationStorage storage)
		{
			LocalizationStorage = storage ?? throw new System.ArgumentNullException(nameof(storage));
			this.window = window;
		}

		public void OnGUI(Rect position)
		{
			EditorGUI.DrawRect(position, background);
			GUI.Label(position, GUIContent.none, "grey_border");

			if (Tag != null)
			{
				var changeCheck = LocalizationStorage.ContainsLocalizationTag(Tag);
				try
				{
					GUILayout.BeginArea(position);
					if (changeCheck) { EditorGUI.BeginChangeCheck(); }
					DrawHeader(Tag);

					scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
					GUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
					DrawResources(Tag, LocalizationStorage.Languages);
					GUILayout.EndVertical();
					GUILayout.EndScrollView();

					if (changeCheck && EditorGUI.EndChangeCheck()) { LocalizationStorage?.ChangeVersion(); }
					GUILayout.EndArea();

					if (notice) { DrawNotice(position); }
				}
				catch (System.ArgumentException){ } 
			}
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
				}
				else
				{
					tag.Resources[i].Data = EditorGUILayout.ObjectField(languages[i].Name, (Object)tag.Resources[i].Data, tag.Type, false, options);
				}
			}
		}

		private void DrawHeader(LocalizationTag localization)
		{
			GUILayout.Space(2);
			GUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
			GUILayout.Label("Name");
			GUILayout.FlexibleSpace();
			localization.Name = GUILayout.TextField(localization.Name);
			GUILayout.FlexibleSpace();

			if (LocalizationStorage.ContainsLocalizationTag(localization))
			{
				if (GUILayout.Button("Delete")) { 
					LocalizationStorage.RemoveLocalizationTag(localization);
					StartNotice(new GUIContent($"{Tag.Name} has been deleted"));
					EditorGUI.FocusTextInControl(null);

				}
			}
			else
			{
				if (GUILayout.Button("Add")) { 
					LocalizationStorage.AddLocalizationTag(localization);
					StartNotice(new GUIContent($"{Tag.Name} has been added"));
					EditorGUI.FocusTextInControl(null);
				}
			}
			GUILayout.EndHorizontal();
			ExtendedEditorGUI.DrawLine(Color.black);
		}

		private void StartNotice(GUIContent content)
		{
			noticeContent = content;
			notice = true;
			curentNotice = 0f;
			lastTime = System.DateTime.Now.Ticks;
		}

		private async void DrawNotice(Rect position)
		{
			long now = System.DateTime.Now.Ticks;
			float deltaTime = (now - lastTime) / (float)System.TimeSpan.TicksPerSecond;
			lastTime = now;

			if (curentNotice != 1f)
			{
				curentNotice = Mathf.MoveTowards(curentNotice, 1f, deltaTime * 4);
			}

			var width = 400f;
			var x = position.x + (position.width - width) * 0.5f;
			var height = 128f;
			var rect = new Rect(x, height * (curentNotice - 0.8f), width, height);

			GUILayout.BeginArea(rect);
			GUILayout.Label(noticeContent, "NotificationBackground");
			GUILayout.EndArea();

			if (curentNotice == 1f)
			{
				await Task.Delay(1000);
				curentNotice = 0f;
				notice = false;
			}

			window.Repaint();
		}
	}
}