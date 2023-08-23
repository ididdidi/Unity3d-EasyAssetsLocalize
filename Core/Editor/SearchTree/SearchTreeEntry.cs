using UnityEngine;

namespace EasyLocalization
{
    /// <summary>
    /// Class for creating a search tree in the Unity editor.
    /// </summary>
    [System.Serializable]
    public class SearchTreeEntry : System.IComparable<SearchTreeEntry>
    {
        public GUIContent Content { get; }
        public string Name => Content.text;
        public int Level { get; }
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
        /// Method for comparing two objects
        /// </summary>
        /// <param name="entry">Base tree element</param>
        /// <returns></returns>
        public int CompareTo(SearchTreeEntry entry) => Name.CompareTo(entry.Name);
    }
}