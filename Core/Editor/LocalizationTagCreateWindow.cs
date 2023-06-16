using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationTagCreateWindow : EditorWindow
	{
		public string TagName { get; set; }
		public System.Action<LocalizationTag> Request { get; set; }
		private string Text { get; set; }
		private Object @object { get; set; }

		private readonly Vector2 size = new Vector2(320f, 200f);

		public void OnEnable()
		{
			minSize = size;
			maxSize = size;
		}

		public static LocalizationTagCreateWindow GetInstance(System.Action<LocalizationTag> request, string tagName = "")
		{
			var window = (LocalizationTagCreateWindow)GetWindow(typeof(LocalizationTagCreateWindow), true, "Create Localization");
			window.TagName = tagName;
			window.Request = request;
			return window;
		}

		private void OnGUI()
		{
			DisplayFields();

			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			CreateButton();
			CancelButton();
			GUILayout.EndHorizontal();

		}

		private void DisplayFields()
		{
			TagName = EditorGUILayout.TextField("Tag name", TagName);
			
			EditorGUILayout.Separator();
			if (!@object) {
				EditorGUILayout.LabelField("Text", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, });
				Text = EditorGUILayout.TextArea(Text, EditorStyles.textArea, GUILayout.MinHeight(50));
			}


			EditorGUILayout.Separator();
			if (!@object && string.IsNullOrEmpty(Text))
			{
				EditorGUILayout.LabelField("OR", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, }, GUILayout.ExpandWidth(true));
			}
			EditorGUILayout.Separator();

			if (string.IsNullOrEmpty(Text)) { @object = EditorGUILayout.ObjectField("Object", @object, typeof(Object), false); }
			EditorGUILayout.Separator();
		}

		private void CreateButton()
		{
			GUI.enabled = CheckProperties();
			if (GUILayout.Button("Create localization")) {
				var resource = @object ? new UnityResource(@object) : new TextResource(Text) as IResource;
				Request?.Invoke(new LocalizationTag(TagName, resource));
				this.Close();
			}
			GUI.enabled = true;
		}

		private void CancelButton()
		{
			if (GUILayout.Button("Cancel")) { this.Close(); }
		}

		private bool CheckProperties()
		{
			return (!string.IsNullOrEmpty(TagName)) && (@object || !string.IsNullOrEmpty(Text));
		}
	}
}