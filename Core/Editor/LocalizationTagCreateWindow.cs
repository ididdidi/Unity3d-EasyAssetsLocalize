using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationTagCreateWindow : EditorWindow
	{
		public LocalizationStorage LocalizationStorage { get; set; }
		private string TagName { get; set; }
		private string Text { get; set; }
		private Object @object { get; set; }

		private Vector2 scrollPosition = Vector2.zero;

		public void OnEnable()
		{
			minSize = new Vector2(240f, 240f);
		}

		private void OnGUI()
		{
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			DisplayFields();

			GUILayout.BeginHorizontal();
			CreateButton();
			CancelButton();
			GUILayout.EndHorizontal();

			EditorGUILayout.EndScrollView();
		}

		private void DisplayFields()
		{
			TagName = EditorGUILayout.TextField("Tag name", TagName);
			if (!@object) {
				EditorGUILayout.LabelField("Text");
				Text = EditorGUILayout.TextArea(Text, EditorStyles.textArea, GUILayout.MinHeight(50));
			}

			if (!@object && string.IsNullOrEmpty(Text))
			{
				EditorGUILayout.LabelField("OR", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, }, GUILayout.ExpandWidth(true));
			}

			if (string.IsNullOrEmpty(Text)) { @object = EditorGUILayout.ObjectField("Object", @object, typeof(Object), false); }
		}

		private void CreateButton()
		{
			GUI.enabled = CheckProperties();
			if (GUILayout.Button("Create localization")) {
				var resource = @object ? new UnityResource(@object) : new TextResource(Text) as IResource;
				LocalizationStorage.AddLocalizationTag(new LocalizationTag(TagName, resource, LocalizationStorage.Languages));
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