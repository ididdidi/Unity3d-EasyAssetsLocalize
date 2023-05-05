using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSearch
    {
        private LocalizationStorage storage;

        private SearchField searchField = new SearchField();
        private string searchMask ="";

        public LocalizationSearch(LocalizationStorage storage)
        {
            this.storage = storage;
        }

        public bool SearchFieldChanged(Rect rect)
        {
            var mask = searchField.OnGUI(rect, searchMask);
            if(GUI.changed && !mask.Equals(searchMask))  
            { 
                searchMask = mask; return true;
            }
            else return false;
        }

        public Localization[] GetResult() => storage?.FindLocalizations(searchMask);
    }
}