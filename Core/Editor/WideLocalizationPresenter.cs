using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public class WideLocalizationPresenter : LocalizationPresenter
    {
        private TypeView typeView;
        //	private NoticeView noticeView;

        public WideLocalizationPresenter(IDisplay parent, SearchTreeView searchView, LocalizationView localizationView, LocalizationPropertiesView propertiesView)
            : base(parent, searchView, localizationView, propertiesView) {
            typeView = new TypeView();
            this.localizationView.OnBackButton = () => searchView?.GoToParent();
            this.propertiesView.OnBackButton = ClosePropertiesView;
			this.searchView.OptionButton = ShowPropertiesButton;
			this.searchView.OnFocusEntry = OnFocusEntry;
            this.searchView.OnSelectEntry = OnSelectEntry;
            //noticeView = new NoticeView(parent);
        }

		/// <summary>
		/// Drawing LocalizationStorage in the inspector window.
		/// </summary>
		public override void OnGUI(Rect position)
		{
			var rect = new Rect(0, 0, 320, position.height);
			EditorGUI.DrawRect(rect, Background);
			if (LocalizationManager.Languages.Length > 0) searchView?.OnGUI(rect);
			
			rect.x = 319;
			rect.width = position.width - 320;
			EditorGUI.DrawRect(rect, Background);

			currentView?.OnGUI(rect);
			rect.width = 1;
			EditorGUI.DrawRect(rect, Color.black);
			//	noticeView.OnGUI();
		}

		/// <summary>
		/// Button to show properties
		/// </summary>
		/// <param name="rect">Position</param>
		private void ShowPropertiesButton(Rect rect)
		{
			if (GUI.Button(rect, EditorGUIUtility.IconContent("_Popup"), GUIStyle.none)) { currentView = propertiesView; }
		}

		private void OnSelectEntry(object data)
		{
			if (data is Localization localization)
			{
				localizationView.Data = localization;
				currentView = localizationView;
			}
		}

		private void OnFocusEntry(object data)
		{
			if (currentView == propertiesView) { return; }
			if (data is GUIContent content)
			{
				typeView.Content = content;
				currentView = typeView;
			}
			else { OnSelectEntry(data); }
		}

		private void ClosePropertiesView()
		{
			currentView = (searchView.CurrentEntry is SearchTreeGroupEntry) ? typeView : localizationView as IEditorView;
		}
	}
}