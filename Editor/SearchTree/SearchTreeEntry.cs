using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class for creating a search tree in the Unity editor.
    /// </summary>
    [System.Serializable]
    public class SearchTreeEntry : System.IComparable<SearchTreeEntry>
    {
        /// <summary>
        /// Content to display in the GUI.
        /// </summary>
        public GUIContent Content { get; }
        /// <summary>
        /// Entry name for find.
        /// </summary>
        public string Name => Content.text;
        /// <summary>
        /// Search tree entry level.
        /// </summary>
        public int Level { get; }
        /// <summary>
        /// Data associated with this entry.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Cotent to display an item in a list</param>
        /// <param name="level">Level in tree</param>
        /// <param name="data">User data</param>
        public SearchTreeEntry(GUIContent content, int level = 0, object data = null)
        {
            Content = content;
            Level = level;
            Data = data;
        }

        /// <summary>
        /// Method for comparing two objects.
        /// </summary>
        /// <param name="entry">Base tree element</param>
        /// <returns></returns>
        public int CompareTo(SearchTreeEntry entry) => Name.CompareTo(entry.Name);
    }
}