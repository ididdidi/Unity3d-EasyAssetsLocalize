using System.Collections.Generic;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class for representing branches of a search tree
    /// </summary>
    [System.Serializable]
    public class SearchTreeGroupEntry : SearchTreeEntry
    {
        public int SelectedIndex;
        public Vector2 ScrollPosition;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Cotent to display an item in a list</param>
        /// <param name="level">Level in tree</param>
        /// <param name="data">User data</param>
        public SearchTreeGroupEntry(GUIContent content, int level = 0, object data = null) : base(content, level, data) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">Cotent to display an item in a list</param>
        /// <param name="level">Level in tree</param>
        /// <param name="data">User data</param>
        public SearchTreeGroupEntry(string content, int level = 0, object data = null) : base(new GUIContent(content), level, data) { }

        /// <summary>
        /// Returns a subset of search tree elements
        /// </summary>
        /// <param name="tree">Set of items to search</param>
        /// <param name="searchKeyIsEmpty">Flag about the absence of a value in the search string</param>
        /// <returns>Subset of search tree elements</returns>
        public SearchTreeEntry[] GetChildren(SearchTreeEntry[] tree, bool searchKeyIsEmpty)
        {
            List<SearchTreeEntry> children = new List<SearchTreeEntry>();
            int level = -1;
            int i = 0;
            while(i < tree.Length)
            {
                if (tree[i++] == this) { level = this.Level + 1; break; }
            }

            if (level == -1) { return children.ToArray(); }

            while (i < tree.Length)
            {
                SearchTreeEntry entry = tree[i++];

                if (entry.Level < level) { break;  }

                if (entry.Level > level && searchKeyIsEmpty) { continue; }
                   
                children.Add(entry);
            }

            return children.ToArray();
        }
    }
}