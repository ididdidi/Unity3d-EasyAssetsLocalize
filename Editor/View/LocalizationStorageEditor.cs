using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
	/// <summary>
	/// Responsible for presenting the Localization Storage in the inspector.
	/// </summary>
	[CustomEditor(typeof(LocalizationStorage))]
	internal partial class LocalizationStorageEditor : Editor, IDisplay
	{
		private NoticeView noticeView;
		private LocalizationStorage storage;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationSettingsView settingsView;
		private LocalizationPresenter localizationPresentor;

		/// <summary>
		/// Storage link caching.
		/// </summary>
		public void OnEnable()
		{
			storage = this.target as LocalizationStorage;

			if (storage.Languages.Count == 0) { storage.AddLanguage(new Language(Application.systemLanguage)); }
			if (storage.Localizations.Length == 0) { storage.CreateDefaultLocalization("Text"); }

			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(storage, noticeView);
			settingsView = new LocalizationSettingsView(storage);
			searchView = new SearchTreeView(this, new LocalizationSearchProvider(storage), false);
			localizationPresentor = new LocalizationPresenter(this, searchView, localizationView, settingsView);
			LocalizationManager.OnStorageChange += OnChangeStorage;
		}

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnInspectorGUI()
		{
			var width = EditorGUIUtility.currentViewWidth;
			var height = Screen.height * (width / Screen.width) - 160;
			GUILayoutUtility.GetRect(width, height);
			var position = new Rect(0, 0, width, height);

			localizationPresentor.OnGUI(position);
			noticeView.OnGUI();
		}

		/// <summary>
		/// Method for displaying data changes.
		/// </summary>
		private void OnChangeStorage(IStorage storage) => searchView.Refresh();

		/// <summary>
		/// This function is called when the behaviour becomes disabled.
		/// </summary>
		private void OnDisable() => LocalizationManager.OnStorageChange -= OnChangeStorage;
	}
}