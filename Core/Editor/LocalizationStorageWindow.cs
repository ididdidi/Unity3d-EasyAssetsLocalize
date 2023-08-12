using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow, IDisplay
	{
		public const float MIN_WIDTH = 720f;
		public const float MIN_HIGHT = 320f;

		private readonly Color LightSkin = new Color(0.77f, 0.77f, 0.77f);
		private readonly Color DarkSkin = new Color(0.22f, 0.22f, 0.22f);

		private Color Background => EditorGUIUtility.isProSkin ? DarkSkin : LightSkin;

		// Data renderer in a editor window
		private IView currentView;
		private TypeCover typePreview;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationPropertiesView settingsView;
		private int storageVersion;
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.Storage; }

		private void OnEnable()
		{
			titleContent = new GUIContent("Simple Localization", EditorGUIUtility.IconContent("FilterByType@2x").image);
			typePreview = new TypeCover();
			localizationView = new LocalizationView(LocalizationStorage, ()=> { });
			settingsView = new LocalizationPropertiesView(ClocePropertiesView); ;
			var provider = new LocalizationSearchProvider(LocalizationStorage, OnSelectEntry, OnFocusEntry);
			searchView = new SearchTreeView(this, provider);
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>(LocalizationStorage.name, true);

			instance.minSize = new Vector2(minWidth, minHight);

			return instance;
		}

		/// <summary>
		/// Method for rendering window content.
		/// </summary>
		internal void OnGUI()
		{
			if(searchView != null && storageVersion != LocalizationStorage.Version)
			{
				searchView.IsChanged = true;
				storageVersion = LocalizationStorage.Version;
			}

			var rect = new Rect(0, 0, 320, position.height);
			EditorGUI.DrawRect(rect, Background);
			if (LocalizationManager.Languages.Length > 0) searchView?.OnGUI(rect);

			if (string.IsNullOrEmpty(searchView?.SearchKeyword)) { ShowPropertiesButton(new Rect(302, 8, 20, 20)); }

			rect.x = 319;
			rect.width = position.width - 320;
			EditorGUI.DrawRect(rect, Background);
			GUI.Label(rect, GUIContent.none, "grey_border");

			currentView?.OnGUI(rect);
		}

		/// <summary>
		/// Button to show properties
		/// </summary>
		/// <param name="rect">Position</param>
		private void ShowPropertiesButton(Rect rect)
		{
			if (GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none)) { currentView = settingsView; }
		}

		private bool OnSelectEntry(object data)
		{
			if (data is Localization localization)
			{
				localizationView.Data = localization;
				currentView = localizationView;
			}
			return false;
		}

		private void OnFocusEntry(object data)
		{
			if(currentView == settingsView) { return; }
			if (data is GUIContent content)
			{
				typePreview.Content = content;
				currentView = typePreview;
			}
			else if (data is Localization localization)
			{
				localizationView.Data = localization;
				currentView = localizationView;
			}
		}

		private void ClocePropertiesView()
		{
			currentView = (searchView.CurrentEntry is SearchTreeGroupEntry) ? typePreview : localizationView as IView;
		}
	}
}