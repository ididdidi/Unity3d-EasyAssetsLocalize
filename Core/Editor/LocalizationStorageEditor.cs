using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	/// <summary>
	/// Responsible for presenting the Localization Storage in the inspector.
	/// </summary>
	[CustomEditor(typeof(LocalizationStorage))]
	public partial class LocalizationStorageEditor : Editor, IDisplay
	{
		private LocalizationStorage storage;
		private SearchEditorView searchView;
		private LocalizationPropertiesView1 propertiesView;
		private IEditorView currentView;

		/// <summary>
		/// Storage link caching.
		/// </summary>
		public void OnEnable()
		{
			storage = this.target as LocalizationStorage;
			searchView = new SearchEditorView(this, new LocalizationSearchProvider(storage, (i) => { Debug.Log(i); return false; }), false);
			searchView.OptionButton = ShowPropertiesButton;
			propertiesView = new LocalizationPropertiesView1();
			currentView = searchView;
		}

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnInspectorGUI()
		{
			var rect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, searchView.HeightInGUI);
			rect.width = EditorGUIUtility.currentViewWidth;
			currentView.OnGUI(rect);
		}

		/// <summary>
		/// Button to show properties
		/// </summary>
		/// <param name="rect">Position</param>
		private void ShowPropertiesButton(Rect rect)
		{
			if (GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none)) { currentView = propertiesView; }
		}
	}
}