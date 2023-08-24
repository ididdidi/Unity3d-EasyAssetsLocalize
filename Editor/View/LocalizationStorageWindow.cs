using UnityEditor;
using UnityEngine;
using EasyAssetsLocalize;

namespace EasyAssetsLocalize
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow, IDisplay
	{
		public const float MIN_WIDTH = 240f;
		public const float MIN_HIGHT = 320f;

		// Data renderer in a editor window
		private NoticeView noticeView;
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private LocalizationPropertiesView propertiesView;
		private LocalizationPresenter localizationPresentor;
		private int storageVersion;
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.Storage; }

		private void OnEnable()
		{
			noticeView = new NoticeView(this);
			localizationView = new LocalizationView(LocalizationStorage, noticeView);
			propertiesView = new LocalizationPropertiesView(LocalizationStorage);
			searchView = new SearchTreeView(this, new LocalizationSearchProvider(LocalizationStorage));
			localizationPresentor = new LocalizationPresenter(this, searchView, localizationView, propertiesView);
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>();
			instance.titleContent = new GUIContent("Simple Localization", EditorGUIUtility.IconContent("FilterByType@2x").image);
			instance.minSize = new Vector2(minWidth, minHight);

			return instance;
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		[MenuItem("Window/Localization Storage")]
		public new static LocalizationStorageWindow Show() => Show(MIN_WIDTH, MIN_HIGHT);

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

			localizationPresentor.OnGUI(new Rect(0, 0, position.width, position.height));

			noticeView.OnGUI();
		}
	}
}