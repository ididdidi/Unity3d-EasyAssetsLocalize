using UnityEngine;
using UnityEditor;

namespace UnityExtended
{
    /// <summary>
    /// Class for creating and displaying a drop-down editor window.
    /// </summary>
    public class SearchDropDownWindow : EditorWindow, IDisplay
    {
        // Defaul size
        private const float defaultWidth = 240f;
        private const float defaultHeight = 320f;

        // Data renderer in a editor window
        private SearchTreeView searchView;

        /// <summary>
        /// Creation of initialization and display of a window on the monitor screen.
        /// </summary>
        /// <param name="provider">SearchTree data provider</param>
        /// <param name="size">Size window</param>
        public static SearchDropDownWindow Show(ISearchTreeProvider provider, System.Action<object> OnSelectEntry, Vector2 size = default)
        {
            float width = System.Math.Max(size.x, defaultWidth);
            float height = System.Math.Max(size.y, defaultHeight);

            Vector2 screenPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Rect buttonRect = new Rect(screenPosition.x - width / 2, screenPosition.y - EditorGUIUtility.singleLineHeight, width, 1);

            var instance = (SearchDropDownWindow)CreateInstance(typeof(SearchDropDownWindow));
            instance.searchView = new SearchTreeView(instance, provider);
            instance.searchView.OnSelectEntry = OnSelectEntry;
            instance.hideFlags = HideFlags.HideAndDontSave;
            instance.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, height));
            instance.Focus();
            instance.wantsMouseMove = true;

            return instance;
        }

        /// <summary>
        /// Method for rendering window content
        /// </summary>
        internal void OnGUI()
        {
            searchView?.OnGUI(position);

            var curentEvent = Event.current;
            if (curentEvent.type == EventType.KeyDown && curentEvent.keyCode == KeyCode.Escape)
            {
                this.Close();
                curentEvent.Use();
            }
        }
    }
}