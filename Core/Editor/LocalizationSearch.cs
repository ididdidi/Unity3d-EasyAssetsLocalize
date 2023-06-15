using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Search for localization resources in a repository by tag name.
    /// </summary>
    public class LocalizationSearch
    {
        private LocalizationStorage storage;

        private SearchField searchField = new SearchField();
        private string key ="";

        public string Key { get => key; set => key = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storage"></param>
        public LocalizationSearch(LocalizationStorage storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Method for rendering the search field.
        /// </summary>
        /// <returns><see>True</see> if there were changes in the field in the current frame</returns>
        public bool SearchFieldChanged()
        {
            var mask = searchField.OnGUI(Key);
            if (GUI.changed && !mask.Equals(Key))
            {
                Key = mask; return true;
            }
            else return false;
        }

        /// <summary>
        /// Method for rendering the search field.
        /// </summary>
        /// <param name="rect"><see>Rect</see> of the search field</param>
        /// <returns><see>True</see> if there were changes in the field in the current frame</returns>
        public bool SearchFieldChanged(Rect rect)
        {
            var mask = searchField.OnGUI(rect, Key);
            if(GUI.changed && !mask.Equals(Key))  
            { 
                Key = mask; return true;
            }
            else return false;
        }

        /// <summary>
        /// Method for getting an array of localizations whose tag names match the query in the search.
        /// </summary>
        /// <returns>Array of localizations</returns>
        public LocalizationTag[] GetResult() => FindLocalizations(Key);


        /// <summary>
        /// Method for searching for localization in storage by localization tag name.
        /// </summary>
        /// <param name="mask">Localization tag name or part of it</param>
        /// <returns>Localization</returns>
        private LocalizationTag[] FindLocalizations(string mask)
        {
            var localizations = storage.LocalizationTags;
            if (string.IsNullOrWhiteSpace(mask)) { return localizations; }

            var result = new List<LocalizationTag>();
            for (int i = 0; i < localizations.Length; i++)
            {
                if (localizations[i].Name.ToLower().Contains(mask.ToLower())) { result.Add(localizations[i]); }
            }
            return result.ToArray();
        }
    }
}