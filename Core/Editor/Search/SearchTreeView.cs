using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SearchTreeView : IEditorView
{
    public interface IContext
    {
        Rect position { get; }
        SearchTreeEntry[] CreateSearchTree();
        bool OnSelectEntry(SearchTreeEntry SearchTreeEntry);
        void Repaint();
        void Close();
    }

    // Styles
    class Styles
    {
        public GUIStyle header = "AC BoldHeader";
        public GUIStyle componentButton = "AC ComponentButton";
        public GUIStyle groupButton = "AC GroupButton";
        public GUIStyle background = "grey_border";
        public GUIStyle rightArrow = "ArrowNavigationRight";
        public GUIStyle leftArrow = "ArrowNavigationLeft";
    }

    private const int k_HeaderHeight = 30;
    private const string kSearchHeader = "Search";


    private static Styles s_Styles;
    private static bool s_DirtyList = false;

    private IContext context;
    private SearchTreeEntry[] m_Tree;
    private SearchTreeEntry[] m_SearchResultTree;
    private List<SearchTreeGroupEntry> m_SelectionStack = new List<SearchTreeGroupEntry>();

    private float m_Anim = 1;
    private int m_AnimTarget = 1;
    private long m_LastTime = 0;
    private bool m_ScrollToSelected = false;
    private string m_DelayedSearch = null;
    private string m_Search = "";

    public SearchTreeView(IContext context)
    {
        this.context = context;
        s_DirtyList = true;
    }

    private bool hasSearch { get { return !string.IsNullOrEmpty(m_Search); } }

    private SearchTreeGroupEntry activeParent
    {
        get
        {
            int index = m_SelectionStack.Count - 2 + m_AnimTarget;

            if (index < 0 || index >= m_SelectionStack.Count)
                return null;

            return m_SelectionStack[index];
        }
    }

    private SearchTreeEntry[] activeTree { get { return hasSearch ? m_SearchResultTree : m_Tree; } }
    private SearchTreeEntry activeSearchTreeEntry
    {
        get
        {
            if (activeTree == null)
                return null;

            List<SearchTreeEntry> children = activeParent.GetChildren(activeTree, hasSearch);
            if (activeParent == null || activeParent.selectedIndex < 0 || activeParent.selectedIndex >= children.Count)
                return null;

            return children[activeParent.selectedIndex];
        }
    }
    private bool isAnimating { get { return m_Anim != m_AnimTarget; } }

    private void CreateSearchTree()
    {
        var tree = context.CreateSearchTree();

        if (tree != null)
            m_Tree = tree;
        else
            m_Tree = new SearchTreeEntry[0];

        // Rebuild stack
        if (m_SelectionStack.Count == 0)
            m_SelectionStack.Add(m_Tree[0] as SearchTreeGroupEntry);
        else
        {
            // The root is always the match for level 0
            SearchTreeGroupEntry match = m_Tree[0] as SearchTreeGroupEntry;
            int level = 0;
            while (true)
            {
                // Assign the match for the current level
                SearchTreeGroupEntry oldSearchTreeEntry = m_SelectionStack[level];
                m_SelectionStack[level] = match;
                m_SelectionStack[level].selectedIndex = oldSearchTreeEntry.selectedIndex;
                m_SelectionStack[level].scroll = oldSearchTreeEntry.scroll;

                // See if we reached last SearchTreeEntry of stack
                level++;
                if (level == m_SelectionStack.Count)
                    break;

                // Try to find a child of the same name as we had before
                List<SearchTreeEntry> children = match.GetChildren(activeTree, hasSearch);
                SearchTreeEntry childMatch = children.FirstOrDefault(c => c.name == m_SelectionStack[level].name);
                if (childMatch != null && childMatch is SearchTreeGroupEntry)
                {
                    match = childMatch as SearchTreeGroupEntry;
                }
                else
                {
                    // If we couldn't find the child, remove all further SearchTreeEntrys from the stack
                    m_SelectionStack.RemoveRange(level, m_SelectionStack.Count - level);
                }
            }
        }

        s_DirtyList = false;
        RebuildSearch();
    }

    private void RebuildSearch()
    {
        if (!hasSearch)
        {
            m_SearchResultTree = null;
            if (m_SelectionStack[m_SelectionStack.Count - 1].name == kSearchHeader)
            {
                m_SelectionStack.Clear();
                m_SelectionStack.Add(m_Tree[0] as SearchTreeGroupEntry);
            }
            m_AnimTarget = 1;
            m_LastTime = System.DateTime.Now.Ticks;
            return;
        }

        // Support multiple search words separated by spaces.
        string[] searchWords = m_Search.ToLower().Split(' ');

        // We keep two lists. Matches that matches the start of an item always get first priority.
        List<SearchTreeEntry> matchesStart = new List<SearchTreeEntry>();
        List<SearchTreeEntry> matchesWithin = new List<SearchTreeEntry>();

        foreach (SearchTreeEntry e in m_Tree)
        {
            if ((e is SearchTreeGroupEntry))
                continue;

            string name = e.name.ToLower().Replace(" ", "");
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
                    matchesStart.Add(e);
                else
                    matchesWithin.Add(e);
            }
        }

        matchesStart.Sort();
        matchesWithin.Sort();

        // Create search tree
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
        // Add parent
        tree.Add(new SearchTreeGroupEntry(new GUIContent(kSearchHeader)));
        // Add search results
        tree.AddRange(matchesStart);
        tree.AddRange(matchesWithin);

        // Create search result tree
        m_SearchResultTree = tree.ToArray();
        m_SelectionStack.Clear();
        m_SelectionStack.Add(m_SearchResultTree[0] as SearchTreeGroupEntry);

        // Always select the first search result when search is changed (e.g. a character was typed in or deleted),
        // because it's usually the best match.
        if (activeParent.GetChildren(activeTree, hasSearch).Count >= 1)
            activeParent.selectedIndex = 0;
        else
            activeParent.selectedIndex = -1;
    }

    public void OnGUI()
    {
        if (s_Styles == null)
            s_Styles = new Styles();

        GUI.Label(new Rect(0, 0, context.position.width, context.position.height), GUIContent.none, s_Styles.background);

        if (s_DirtyList)
            CreateSearchTree();

        // Keyboard
        HandleKeyboard();

        GUILayout.Space(7);

        // Search
        EditorGUI.FocusTextInControl("ComponentSearch");

        Rect searchRect = GUILayoutUtility.GetRect(10, 20);
        searchRect.x += 8;
        searchRect.width -= 16;

        GUI.SetNextControlName("ComponentSearch");

        EditorGUI.BeginChangeCheck();

        string newSearch = UnityExtended.ExtendedEditorGUI.SearchField(searchRect, m_DelayedSearch ?? m_Search);

        if (EditorGUI.EndChangeCheck() && (newSearch != m_Search || m_DelayedSearch != null))
        {
            if (!isAnimating)
            {
                m_Search = m_DelayedSearch ?? newSearch;
                RebuildSearch();
                m_DelayedSearch = null;
            }
            else
            {
                m_DelayedSearch = newSearch;
            }
        }

        // Show lists
        ListGUI(activeTree, m_Anim, GetSearchTreeEntryRelative(0), GetSearchTreeEntryRelative(-1));
        if (m_Anim < 1)
            ListGUI(activeTree, m_Anim + 1, GetSearchTreeEntryRelative(-1), GetSearchTreeEntryRelative(-2));

        // Animate
        if (isAnimating && Event.current.type == EventType.Repaint)
        {
            long now = System.DateTime.Now.Ticks;
            float deltaTime = (now - m_LastTime) / (float)System.TimeSpan.TicksPerSecond;
            m_LastTime = now;
            m_Anim = Mathf.MoveTowards(m_Anim, m_AnimTarget, deltaTime * 4);
            if (m_AnimTarget == 0 && m_Anim == 0)
            {
                m_Anim = 1;
                m_AnimTarget = 1;
                m_SelectionStack.RemoveAt(m_SelectionStack.Count - 1);
            }
            context?.Repaint();
        }
    }

    private void ListGUI(SearchTreeEntry[] tree, float anim, SearchTreeGroupEntry parent, SearchTreeGroupEntry grandParent)
    {
        // Smooth the fractional part of the anim value
        anim = Mathf.Floor(anim) + Mathf.SmoothStep(0, 1, Mathf.Repeat(anim, 1));

        // Calculate rect for animated area
        Rect animRect = context.position;
        animRect.x = context.position.width * (1 - anim) + 1;
        animRect.y = k_HeaderHeight;
        animRect.height -= k_HeaderHeight;
        animRect.width -= 2;

        // Start of animated area (the part that moves left and right)
        GUILayout.BeginArea(animRect);

        // Header
        Rect headerRect = GUILayoutUtility.GetRect(10, 25);
        string name = parent.name;
        GUI.Label(headerRect, name, s_Styles.header);

        // Back button
        if (grandParent != null)
        {
            float yOffset = (headerRect.height - s_Styles.leftArrow.fixedHeight) / 2;
            Rect arrowRect = new Rect(
                headerRect.x + s_Styles.leftArrow.margin.left,
                headerRect.y + yOffset,
                s_Styles.leftArrow.fixedWidth,
                s_Styles.leftArrow.fixedHeight);
            if (Event.current.type == EventType.Repaint)
                s_Styles.leftArrow.Draw(arrowRect, false, false, false, false);
            if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
            {
                GoToParent();
                Event.current.Use();
            }
        }

        ListGUI(tree, parent);

        GUILayout.EndArea();
    }

    private void SelectEntry(SearchTreeEntry e, bool shouldInvokeCallback)
    {
        if (e is SearchTreeGroupEntry)
        {
            if (!hasSearch)
            {
                m_LastTime = System.DateTime.Now.Ticks;
                if (m_AnimTarget == 0)
                    m_AnimTarget = 1;
                else if (m_Anim == 1)
                {
                    m_Anim = 0;
                    m_SelectionStack.Add(e as SearchTreeGroupEntry);
                }
            }
        }
        else if (shouldInvokeCallback && context.OnSelectEntry(e))
            context.Close();
    }

    private void ListGUI(SearchTreeEntry[] tree, SearchTreeGroupEntry parent)
    {
        // Start of scroll view list
        parent.scroll = GUILayout.BeginScrollView(parent.scroll);

        EditorGUIUtility.SetIconSize(new Vector2(16, 16));

        List<SearchTreeEntry> children = parent.GetChildren(tree, hasSearch);

        Rect selectedRect = new Rect();

        // Iterate through the children
        for (int i = 0; i < children.Count; i++)
        {
            SearchTreeEntry e = children[i];
            Rect r = GUILayoutUtility.GetRect(16, 20, GUILayout.ExpandWidth(true));

            // Select the SearchTreeEntry the mouse cursor is over.
            // Only do it on mouse move - keyboard controls are allowed to overwrite this until the next time the mouse moves.
            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown)
            {
                if (parent.selectedIndex != i && r.Contains(Event.current.mousePosition))
                {
                    parent.selectedIndex = i;
                    context?.Repaint();
                }
            }

            bool selected = false;
            // Handle selected item
            if (i == parent.selectedIndex)
            {
                selected = true;
                selectedRect = r;
            }

            // Draw SearchTreeEntry
            if (Event.current.type == EventType.Repaint)
            {
                GUIStyle labelStyle = (e is SearchTreeGroupEntry) ? s_Styles.groupButton : s_Styles.componentButton;
                labelStyle.Draw(r, e.content, false, false, selected, selected);
                if ((e is SearchTreeGroupEntry))
                {
                    float yOffset = (r.height - s_Styles.rightArrow.fixedHeight) / 2;
                    Rect arrowRect = new Rect(
                        r.xMax - s_Styles.rightArrow.fixedWidth - s_Styles.rightArrow.margin.right,
                        r.y + yOffset,
                        s_Styles.rightArrow.fixedWidth,
                        s_Styles.rightArrow.fixedHeight);
                    s_Styles.rightArrow.Draw(arrowRect, false, false, false, false);
                }
            }
            if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                parent.selectedIndex = i;
                SelectEntry(e, true);
            }
        }

        EditorGUIUtility.SetIconSize(Vector2.zero);

        GUILayout.EndScrollView();

        // Scroll to show selected
        if (m_ScrollToSelected && Event.current.type == EventType.Repaint)
        {
            m_ScrollToSelected = false;
            Rect scrollRect = GUILayoutUtility.GetLastRect();
            if (selectedRect.yMax - scrollRect.height > parent.scroll.y)
            {
                parent.scroll.y = selectedRect.yMax - scrollRect.height;
                context?.Repaint();
            }
            if (selectedRect.y < parent.scroll.y)
            {
                parent.scroll.y = selectedRect.y;
                context?.Repaint();
            }
        }
    }

    private SearchTreeGroupEntry GetSearchTreeEntryRelative(int rel)
    {
        int i = m_SelectionStack.Count + rel - 1;
        if (i < 0 || i >= m_SelectionStack.Count)
            return null;
        return m_SelectionStack[i] as SearchTreeGroupEntry;
    }

    private void GoToParent()
    {
        if (m_SelectionStack.Count > 1)
        {
            m_AnimTarget = 0;
            m_LastTime = System.DateTime.Now.Ticks;
        }
    }

    private void HandleKeyboard()
    {
        Event evt = Event.current;
        if (evt.type == EventType.KeyDown)
        {
            // Always do these
            if (evt.keyCode == KeyCode.DownArrow)
            {
                activeParent.selectedIndex++;
                activeParent.selectedIndex = Mathf.Min(activeParent.selectedIndex, activeParent.GetChildren(activeTree, hasSearch).Count - 1);
                m_ScrollToSelected = true;
                evt.Use();
            }
            if (evt.keyCode == KeyCode.UpArrow)
            {
                activeParent.selectedIndex--;
                activeParent.selectedIndex = Mathf.Max(activeParent.selectedIndex, 0);
                m_ScrollToSelected = true;
                evt.Use();
            }
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                if (activeSearchTreeEntry != null)
                {
                    SelectEntry(activeSearchTreeEntry, true);
                    evt.Use();
                }
            }

            // Do these if we're not in search mode
            if (!hasSearch)
            {
                if (evt.keyCode == KeyCode.LeftArrow || evt.keyCode == KeyCode.Backspace)
                {
                    GoToParent();
                    evt.Use();
                }
                if (evt.keyCode == KeyCode.RightArrow)
                {
                    if (activeSearchTreeEntry != null)
                    {
                        SelectEntry(activeSearchTreeEntry, false);
                        evt.Use();
                    }
                }
                if (evt.keyCode == KeyCode.Escape)
                {
                    context.Close();
                    evt.Use();
                }
            }
        }
    }
}
