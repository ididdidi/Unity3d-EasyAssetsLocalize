using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public abstract class LocalizationPresenter
	{
		private readonly Color LightSkin = new Color(0.77f, 0.77f, 0.77f);
		private readonly Color DarkSkin = new Color(0.22f, 0.22f, 0.22f);
		protected Color Background => EditorGUIUtility.isProSkin ? DarkSkin : LightSkin;

		protected IDisplay parent;
		protected IEditorView currentView;

		protected SearchTreeView searchView;
		protected LocalizationView localizationView;
		protected LocalizationPropertiesView propertiesView;

		public LocalizationPresenter(IDisplay parent, SearchTreeView searchView, LocalizationView localizationView, LocalizationPropertiesView propertiesView)
		{
			this.parent = parent ?? throw new System.ArgumentNullException(nameof(parent));
			this.searchView = searchView ?? throw new System.ArgumentNullException(nameof(searchView));
			this.localizationView = localizationView ?? throw new System.ArgumentNullException(nameof(localizationView));
			this.propertiesView = propertiesView ?? throw new System.ArgumentNullException(nameof(propertiesView));
		}

		public abstract void OnGUI(Rect position);
	}
}