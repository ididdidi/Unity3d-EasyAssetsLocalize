using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow
	{
		public const float MIN_WIDTH = 720f;
		public const float MIN_HIGHT = 320f;

		public Color background = new Color(0.22f, 0.22f, 0.22f);

		// Data renderer in a editor window
		private SearchTreeView searchView;
		private LocalizationView localizationView;
		private int storageVersion;
		
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.LocalizationStorage; }

		private void OnEnable()
		{
			localizationView = new LocalizationView(this, LocalizationStorage);
			var provider = new LocalizationSearchProvider(LocalizationStorage, (tag) => { localizationView.Tag = tag; return false; }, (tag) => localizationView.Tag = tag);
			searchView = new SearchTreeView(this, provider);

			//hideFlags = HideFlags.HideAndDontSave;
			//wantsMouseMove = true;
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>(LocalizationStorage.name);

			instance.minSize = new Vector2(minWidth, minHight);

			return instance;
		}

		/// <summary>
		/// Method for rendering window content
		/// </summary>
		internal void OnGUI()
		{
			if(searchView != null && storageVersion != LocalizationStorage.Version)
			{
				searchView.IsChanged = true;
				storageVersion = LocalizationStorage.Version;
			}

			var rect = new Rect(0, 0, 320, position.height);
			searchView?.OnGUI(rect);

			rect.x = 319;
			rect.width = position.width - 320;
			localizationView?.OnGUI(rect);
		}
	}
}