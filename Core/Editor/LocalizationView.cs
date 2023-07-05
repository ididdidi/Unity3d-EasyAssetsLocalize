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
		GUIContent content;
		long lastTime;
		float currentAnimation, targetAnimation, wait = 0f;

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

					DrawNotice(position);
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

					content = new GUIContent($"{Tag.Name} has been deleted");
					EditorGUI.FocusTextInControl(null);
					targetAnimation = 1f;
					lastTime = System.DateTime.Now.Ticks;
				}
			}
			else
			{
				if (GUILayout.Button("Add")) { 
					LocalizationStorage.AddLocalizationTag(localization);


					content = new GUIContent($"{Tag.Name} has been added");
					EditorGUI.FocusTextInControl(null);
					targetAnimation = 1f;
					lastTime = System.DateTime.Now.Ticks;
				}
			}
			GUILayout.EndHorizontal();
			ExtendedEditorGUI.DrawLine(Color.black);
		}

		private void DrawNotice(Rect position)
		{
			if (targetAnimation != 1f) return;

			long now = System.DateTime.Now.Ticks;
			float deltaTime = (now - lastTime) / (float)System.TimeSpan.TicksPerSecond;
			lastTime = now;

			if (currentAnimation != targetAnimation)
			{
				currentAnimation = Mathf.MoveTowards(currentAnimation, targetAnimation, deltaTime);
			}
			else if (currentAnimation == 1f)
			{
				wait = Mathf.MoveTowards(wait, targetAnimation, deltaTime);
				if(wait == targetAnimation)
				{
					currentAnimation = 0f;
					targetAnimation = 0f;
					wait = 0f;
				}
			}

			var w = 400f;
			var x = position.x + (position.width - w) * 0.5f;
			var y = 128f;
			var rect = new Rect(x, y * (currentAnimation - 0.8f), w, y);

			GUILayout.BeginArea(rect);
			GUILayout.Label(content, "NotificationBackground");
			GUILayout.EndArea();

			window.Repaint();
		}
	}
}