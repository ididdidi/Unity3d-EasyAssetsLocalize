using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace ResourceLocalization
{
	/// <summary>
	/// Display localization storage data in a separate inspector window.
	/// </summary>
	public class LocalizationStorageWindow : EditorWindow, IContext
	{
		public const float MIN_WIDTH = 720f;
		public const float MIN_HIGHT = 320f;

		public Color background = new Color(0.22f, 0.22f, 0.22f);

		// Data renderer in a editor window
		public IEditorView View { get; set; }
		private SearchTreeView searchView;
		private int storageVersion;
		
		private static LocalizationStorage LocalizationStorage { get => LocalizationManager.LocalizationStorage; }

		private void OnEnable()
		{
			var localizationView = new LocalizationView(LocalizationStorage);
			searchView = new SearchTreeView(new LocalizationSearchProvider(LocalizationStorage, (tag) => { localizationView.Tag = tag; return false; }, (tag) => localizationView.Tag = tag));
			View = new TwoPaneView(searchView, localizationView, 3f, 5f);
		}

		/// <summary>
		/// Creation of initialization and display of a window on the monitor screen.
		/// </summary>
		public static LocalizationStorageWindow Show(float minWidth = MIN_WIDTH, float minHight = MIN_HIGHT)
		{
			var instance = GetWindow<LocalizationStorageWindow>(LocalizationStorage.name);

			instance.hideFlags = HideFlags.HideAndDontSave;
			instance.wantsMouseMove = true;
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
			View?.OnGUI(this);
		}

		public new void Close() { }
	}
}