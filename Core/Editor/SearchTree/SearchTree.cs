using System.Collections.Generic;
using System.Linq;

namespace EasyLocalization
{
    /// <summary>
    /// Encapsulates the current search tree.
    /// </summary>
    public class SearchTree
    {
        private const string searchHeader = "Search";

        public string keyword = string.Empty;
        public SearchTreeEntry[] entries;
        public SearchTreeEntry[] searchResultTree;
        public List<SearchTreeGroupEntry> selectionStack = new List<SearchTreeGroupEntry>();

        public bool SearchKeyIsEmpty { get { return string.IsNullOrEmpty(keyword); } }
        public SearchTreeEntry[] ActiveTree { get => (!SearchKeyIsEmpty) ? searchResultTree : entries; }

        /// <summary>
        /// Creating a tree for search
        /// </summary>
        public void BuildStack(SearchTreeEntry[] entries)
        {
            this.entries = entries ?? new SearchTreeEntry[0];

            if (selectionStack.Count == 0)
                selectionStack.Add(entries[0] as SearchTreeGroupEntry);
            else
            {
                // The root is always the match for level 0
                SearchTreeGroupEntry match = entries[0] as SearchTreeGroupEntry;
                int level = 0;
                while (level < selectionStack.Count)
                {
                    // Assign the match for the current level
                    SearchTreeGroupEntry oldSearchTreeEntry = selectionStack[level];
                    selectionStack[level] = match;
                    selectionStack[level].SelectedIndex = oldSearchTreeEntry.SelectedIndex;
                    selectionStack[level].ScrollPosition = oldSearchTreeEntry.ScrollPosition;

                    // See if we reached last SearchTreeEntry of stack
                    level++;
                    if (level == selectionStack.Count)
                        break;

                    // Try to find a child of the same name as we had before
                    SearchTreeEntry[] children = match.GetChildren(ActiveTree, SearchKeyIsEmpty);
                    SearchTreeEntry childMatch = children.FirstOrDefault(entry => entry.Name == selectionStack[level].Name);
                    if (childMatch != null && childMatch is SearchTreeGroupEntry)
                    {
                        match = childMatch as SearchTreeGroupEntry;
                    }
                    else
                    {
                        // If we couldn't find the child, remove all further SearchTreeEntrys from the stack
                        selectionStack.RemoveRange(level, selectionStack.Count - level);
                    }
                }
            }

            Update();
        }

        /// <summary>
        /// Rebuil a search tree
        /// </summary>
        public void Update()
        {
            if (SearchKeyIsEmpty)
            {
                searchResultTree = null;
                if (selectionStack[selectionStack.Count - 1].Name == searchHeader)
                {
                    selectionStack.Clear();
                    selectionStack.Add(entries[0] as SearchTreeGroupEntry);
                }
                return;
            }

            // Support multiple search words separated by spaces.
            string[] searchWords = keyword.ToLower().Split(' ');

            // We keep two lists. Matches that matches the start of an item always get first priority.
            List<SearchTreeEntry> matchesStart = new List<SearchTreeEntry>();
            List<SearchTreeEntry> matchesWithin = new List<SearchTreeEntry>();

            foreach (SearchTreeEntry entry in entries)
            {
                if (entry is SearchTreeGroupEntry)
                    continue;

                string name = entry.Name.ToLower().Replace(" ", "");
                bool didMatchAll = true;
                bool didMatchStart = false;

                // See if we match ALL the seaarch words.
                for (int w = 0; w < searchWords.Length; w++)
                {
                    string search = searchWords[w];
                    if (name.Contains(search))
                    {
                        // If the start of the item matches the first search word, make a note of that.
                        if (w == 0 && name.StartsWith(search))
                            didMatchStart = true;
                    }
                    else
                    {
                        // As soon as any word is not matched, we disregard this item.
                        didMatchAll = false;
                        break;
                    }
                }
                // We always need to match all search words.
                // If we ALSO matched the start, this item gets priority.
                if (didMatchAll)
                {
                    if (didMatchStart)
                        matchesStart.Add(entry);
                    else
                        matchesWithin.Add(entry);
                }
            }

            matchesStart.Sort();
            matchesWithin.Sort();

            // Create search tree
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            // Add parent
            tree.Add(new SearchTreeGroupEntry(searchHeader));
            // Add search results
            tree.AddRange(matchesStart);
            tree.AddRange(matchesWithin);

            // Create search result tree
            searchResultTree = tree.ToArray();
            selectionStack.Clear();
            selectionStack.Add(searchResultTree[0] as SearchTreeGroupEntry);
        }

        /// <summary>
        /// Method for getting the parent entries of the search tree
        /// </summary>
        /// <param name="relativelayer"></param>
        /// <returns>Return parent group entry if it exists</returns>
        public SearchTreeGroupEntry GetReturnGroupEntry(int relativelayer)
        {
            int i = selectionStack.Count + relativelayer - 1;
            if (i < 0 || i >= selectionStack.Count) { return null; }
            else { return selectionStack[i] as SearchTreeGroupEntry; }
        }
    }
}