using UnityEditor;
using UnityEngine;

namespace ResourceLocalization
{
	public class LocalizationCreateWindow : EditorWindow
	{
		public LocalizationStorage LocalizationStorage { get; set; }
		private string TagName { get; set; }
		private Object Resource { get; set; }

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
			Resource = EditorGUILayout.ObjectField("Resource", Resource, typeof(Object), false);
		}

		private void CreateButton()
		{
			GUI.enabled = CheckProperties();
			if (GUILayout.Button("Create localization")) {
				LocalizationStorage.AddLocalization(TagName, Resource);
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
			return (!string.IsNullOrEmpty(TagName)) && Resource;
		}
	}
}