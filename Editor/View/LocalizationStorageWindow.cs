using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	internal class LocalizationStorageWindow : EditorWindow, IDisplay
	{
		public const float MIN_WIDTH = 240f;
		public const float MIN_HIGHT = 320f;

		// Data renderer in a editor window
		private NoticeView noticeView;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationSettingsView settingsView;
		private LocalizationPresenter localizationPresentor;
		private IStorage storage;
		private IStorage Storage { get => storage ?? LocalizationController.Storage; }

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(IStorage storage, float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>();
			instance.titleContent = new GUIContent("Easy Assets Localize", EditorGUIUtility.IconContent("FilterByType@2x").image);
			instance.minSize = new Vector2(minWidth, minHight);
			instance.storage = storage;
			instance.Initialize();
			return instance;
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		[MenuItem("Window/Localization Storage")]
		public new static LocalizationStorageWindow Show() => Show(Resources.Load<LocalizationStorage>(nameof(LocalizationStorage)));

		/// <summary>
		/// Method for initialization
		/// </summary>
		private void Initialize()
		{
			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(Storage, noticeView);
			settingsView = new LocalizationSettingsView(Storage);
			searchView = new SearchTreeView(this, new LocalizationSearchProvider(Storage));
			localizationPresentor = new LocalizationPresenter(this, searchView, localizationView, settingsView);
			Storage.OnChange += OnChangeStorage;
		}

		/// <summary>
		/// Method for rendering window content.
		/// </summary>
		internal void OnGUI()
		{
			localizationPresentor.OnGUI(new Rect(0, 0, position.width, position.height));
			noticeView.OnGUI();
		}

		/// <summary>
		/// Method for displaying data changes.
		/// </summary>
		private void OnChangeStorage() => searchView.IsChanged = true;

		/// <summary>
		/// This function is called when the behaviour becomes disabled.
		/// </summary>
		private void OnDisable() => Storage.OnChange -= OnChangeStorage;
	}
}