using UnityEditor;
using UnityEngine;

namespace EasyLocalization
{
    public class SearchTreeView : IEditorView
    {
        // Styles
        static class Styles
        {
            public static GUIStyle header = "AC BoldHeader";
            public static GUIStyle componentButton = "AC ComponentButton";
            public static GUIStyle groupButton = "AC GroupButton";
            public static GUIStyle border = "grey_border";
            public static GUIStyle rightArrow = "ArrowNavigationRight";
            public static GUIStyle leftArrow = "ArrowNavigationLeft";
        }

        #region Fields
        // Constants
        private const int headerHeight = 30;

        // Member variables
        private SearchTree searchTree;
        private IDisplay display;
        private ISearchTreeProvider provider;
        private bool grabFocus;
        private int controlId;

        // Animation variables
        private string delayedSearch = null;
        private float currentAnimation = 1;
        private int targetAnimation = 1;
        private long lastTime = 0;
        private bool scrollToSelected = false;
        #endregion

        // Constructor
        public SearchTreeView(IDisplay display, ISearchTreeProvider provider, bool grabFocus = true)
        {
            this.display = display;
            this.provider = provider;
            this.grabFocus = grabFocus;
            IsChanged = true;
        }

        #region Properties
        public bool IsChanged { get; set; }
        public string SearchKeyword => searchTree.keyword;
        public SearchTreeEntry CurrentEntry { get; private set; }
        public System.Action<object> OnFocusEntry { get; set; }
        public System.Action<object> OnSelectEntry { get; set; }
        public System.Action<Rect> OptionButton { get; set; }
        private SearchTreeGroupEntry ActiveParent
        {
            get
            {
                int index = searchTree.selectionStack.Count - 2 + targetAnimation;

                if (index < 0 || index >= searchTree.selectionStack.Count)
                    return null;

                return searchTree.selectionStack[index];
            }
        }
        private SearchTreeEntry[] ActiveChildren => ActiveParent.GetChildren(searchTree.ActiveTree, searchTree.SearchKeyIsEmpty);
        private SearchTreeEntry ActiveSearchEntry
        {
            get
            {
                if (searchTree.ActiveTree == null)
                    return null;

                SearchTreeEntry[] children = ActiveChildren;
                if (ActiveParent == null || ActiveParent.SelectedIndex < 0 || ActiveParent.SelectedIndex >= children.Length)
                    return null;

                return children[ActiveParent.SelectedIndex];
            }
        }
        private bool isAnimating { get => currentAnimation != targetAnimation; }
        private bool isOnFocus { get => GUIUtility.keyboardControl == controlId || GUIUtility.keyboardControl == 0; }
        #endregion

        /// <summary>
        /// Method for go to previous level
        /// </summary>
        public void GoToParent()
        {
            if (searchTree.selectionStack.Count > 1)
            {
                targetAnimation = 0;
                lastTime = System.DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Method to display search field and tree.
        /// </summary>
        /// <param name="context">Object for interacting with an object in which data is displayed</param>
        public void OnGUI(Rect position)
        {
            if (IsChanged)
            {
                searchTree = provider.GetSearchTree();
                if (searchTree.SearchKeyIsEmpty)
                {
                    targetAnimation = 1;
                    lastTime = System.DateTime.Now.Ticks;
                }

                // Check SelectedIndex
                var countActiveChildren = ActiveChildren.Length;
                if (ActiveParent.SelectedIndex >= countActiveChildren)
                {
                    ActiveParent.SelectedIndex = countActiveChildren - 1;
                }
                else if (ActiveParent.SelectedIndex < 0 && countActiveChildren > 0)
                {
                    ActiveParent.SelectedIndex = 0;
                }

                IsChanged = false;
            }

            // Keyboard
            HandleKeyboard(Event.current);

            // Search
            Rect searchRect = new Rect(8, 8, position.width - 16, 20);

            GUI.SetNextControlName("SearchField");
            EditorGUI.BeginChangeCheck();
            var newSearch = ExtendedEditor.SearchField(searchRect, delayedSearch ?? searchTree.keyword, out controlId);
            if (EditorGUI.EndChangeCheck() && (newSearch != searchTree.keyword || delayedSearch != null))
            {
                if (!isAnimating)
                {
                    searchTree.keyword = delayedSearch ?? newSearch;
                    searchTree.Update();
                    // Always select the first search result when search is changed (e.g. a character was typed in or deleted),
                    // because it's usually the best match.
                    if (ActiveChildren.Length >= 1) { ActiveParent.SelectedIndex = 0; }
                    else { ActiveParent.SelectedIndex = -1; }
                    delayedSearch = null;
                }
                else { delayedSearch = newSearch; }
            }

            if (grabFocus) { grabFocus = false; EditorGUI.FocusTextInControl("SearchField"); }

            if (OptionButton != null && string.IsNullOrEmpty(searchTree.keyword))
            {
                OptionButton.Invoke(new Rect(position.width - 18, 8, 20, 20));
            }
        
            // Show lists
            ListGUI(position, searchTree.ActiveTree, currentAnimation, searchTree.GetReturnGroupEntry(0), searchTree.GetReturnGroupEntry(-1));
            if (currentAnimation < 1) { ListGUI(position, searchTree.ActiveTree, currentAnimation + 1, searchTree.GetReturnGroupEntry(-1), searchTree.GetReturnGroupEntry(-2)); }
        
            FocusEntry(ActiveSearchEntry);
        
            // Animate
            if (isAnimating && Event.current.type == EventType.Repaint)
            {
                long now = System.DateTime.Now.Ticks;
                float deltaTime = (now - lastTime) / (float)System.TimeSpan.TicksPerSecond;
                lastTime = now;
                currentAnimation = Mathf.MoveTowards(currentAnimation, targetAnimation, deltaTime * 4);
                if (targetAnimation == 0 && currentAnimation == 0)
                {
                    currentAnimation = 1;
                    targetAnimation = 1;
                    searchTree.selectionStack.RemoveAt(searchTree.selectionStack.Count - 1);
                }
                display.Repaint();
            }
        }

        /// <summary>
        /// Method for animated displaying search list.
        /// </summary>
        /// <param name="context">Object for interacting with an object in which data is displayed</param>
        /// <param name="tree">Search tree</param>
        /// <param name="animationValue">Animation value at the moment</param>
        /// <param name="parent">The parent entry of the search tree</param>
        /// <param name="grandParent">The parent of the parent entry</param>
        private void ListGUI(Rect position, SearchTreeEntry[] tree, float animationValue, SearchTreeGroupEntry parent, SearchTreeGroupEntry grandParent)
        {
            // Smooth the fractional part of the animation value
            animationValue = Mathf.Floor(animationValue) + Mathf.SmoothStep(0, 1, Mathf.Repeat(animationValue, 1));

            // Calculate rect for animated area
            Rect animRect = position;
            animRect.x = position.width * (1 - animationValue) + 1;
            animRect.y = headerHeight;
            animRect.height -= headerHeight;
            animRect.width -= 2;

            // Start of animated area (the part that moves left and right)
            GUILayout.BeginArea(animRect);

            // Header
            Rect headerRect = GUILayoutUtility.GetRect(10, 25);
            string name = parent.Name;
            GUI.Label(headerRect, name, Styles.header);

            // Back button
            if (grandParent != null)
            {
                float yOffset = (headerRect.height - Styles.leftArrow.fixedHeight) / 2;
                Rect arrowRect = new Rect(
                    headerRect.x + Styles.leftArrow.margin.left,
                    headerRect.y + yOffset,
                    Styles.leftArrow.fixedWidth,
                    Styles.leftArrow.fixedHeight);
                if (Event.current.type == EventType.Repaint) { Styles.leftArrow.Draw(arrowRect, false, false, false, false); }

                if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
                {
                    GoToParent();
                    Event.current.Use();
                }
            }

            ListGUI(tree, parent);
            GUILayout.EndArea();
        }

        /// <summary>
        /// Method directly to display the search list
        /// </summary>
        /// <param name="context">Object for interacting with an object in which data is displayed</param>
        /// <param name="tree">Search tree</param>
        /// <param name="parent">The parent entry of the search tree</param>
        private void ListGUI(SearchTreeEntry[] tree, SearchTreeGroupEntry parent)
        {
            var height = EditorGUIUtility.singleLineHeight;
            EditorGUIUtility.SetIconSize(new Vector2(height, height));

            // Start of scroll view list
            parent.ScrollPosition = GUILayout.BeginScrollView(parent.ScrollPosition);

            SearchTreeEntry[] children = parent.GetChildren(tree, searchTree.SearchKeyIsEmpty);

            Rect selectedRect = new Rect();

            // Iterate through the children
            for (int i = 0; i < children.Length; i++)
            {
                SearchTreeEntry entry = children[i];
                Rect entryRect = GUILayoutUtility.GetRect(height, height + 2, GUILayout.ExpandWidth(true));

                // Select the SearchTreeEntry the mouse cursor is over.
                // Only do it on mouse move - keyboard controls are allowed to overwrite this until the next time the mouse moves.
                if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown)
                {
                    if (parent.SelectedIndex != i && entryRect.Contains(Event.current.mousePosition))
                    {
                        parent.SelectedIndex = i;
                        display.Repaint();
                    }
                }

                if (Event.current.type == EventType.MouseDown)
                {
                    // Reset Focus
                    EditorGUI.FocusTextInControl(null);
                    if (entryRect.Contains(Event.current.mousePosition))
                    {
                        Event.current.Use();
                        parent.SelectedIndex = i;
                        SelectEntry(entry);
                    }
                }

                bool selected = false;
                // Handle selected item
                if (i == parent.SelectedIndex)
                {
                    selected = true;
                    selectedRect = entryRect;
                }

                // Draw SearchTreeEntry
                if (Event.current.type == EventType.Repaint)
                {
                    GUIStyle labelStyle = (entry is SearchTreeGroupEntry) ? Styles.groupButton : Styles.componentButton;
                    labelStyle.Draw(entryRect, entry.Content, false, false, selected, selected);
                    if (entry is SearchTreeGroupEntry)
                    {
                        float yOffset = (entryRect.height - Styles.rightArrow.fixedHeight) / 2;
                        Rect arrowRect = new Rect(
                            entryRect.xMax - Styles.rightArrow.fixedWidth - Styles.rightArrow.margin.right,
                            entryRect.y + yOffset,
                            Styles.rightArrow.fixedWidth,
                            Styles.rightArrow.fixedHeight);
                        Styles.rightArrow.Draw(arrowRect, false, false, false, false);
                    }
                }
            }

            EditorGUIUtility.SetIconSize(Vector2.zero);
            GUILayout.EndScrollView();

            // Scroll to show selected
            if (scrollToSelected && Event.current.type == EventType.Repaint)
            {
                scrollToSelected = false;
                Rect scrollRect = GUILayoutUtility.GetLastRect();
                if (selectedRect.yMax - scrollRect.height > parent.ScrollPosition.y)
                {
                    parent.ScrollPosition.y = selectedRect.yMax - scrollRect.height;
                    display.Repaint();
                }
                if (selectedRect.y < parent.ScrollPosition.y)
                {
                    parent.ScrollPosition.y = selectedRect.y;
                    display.Repaint();
                }
            }
        }

        private void FocusEntry(SearchTreeEntry entry)
        {
            if (!isAnimating && CurrentEntry != entry)
            {
                CurrentEntry = entry;
                OnFocusEntry?.Invoke(CurrentEntry?.Data ?? new GUIContent("Not found...", EditorGUIUtility.IconContent("Search Icon").image));
            }
        }

        /// <summary>
        /// Action associated with the selected item.
        /// </summary>
        /// <param name="entry">Selected search tree entry</param>
        private void SelectEntry(SearchTreeEntry entry)
        {
            if (entry is SearchTreeGroupEntry group)
            {
                if (searchTree.SearchKeyIsEmpty)
                {
                    lastTime = System.DateTime.Now.Ticks;
                    if (targetAnimation == 0) { targetAnimation = 1; }
                    else if (currentAnimation == 1)
                    {
                        currentAnimation = 0;
                        searchTree.selectionStack.Add(group);
                    }
                }
            }
            else { OnSelectEntry?.Invoke(entry.Data); }
        }

        /// <summary>
        /// Handles keystrokes
        /// </summary>
        /// <param name="curentEvent"></param>
        private void HandleKeyboard(Event curentEvent)
        {
            if (curentEvent.type == EventType.KeyDown && isOnFocus)
            {
                switch (curentEvent.keyCode)
                {
                    case KeyCode.PageUp:
                        {
                            ActiveParent.SelectedIndex--;
                            ActiveParent.SelectedIndex = 0;
                            scrollToSelected = true;
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.PageDown:
                        {
                            ActiveParent.SelectedIndex = ActiveChildren.Length - 1;
                            scrollToSelected = true;
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.UpArrow:
                        {
                            ActiveParent.SelectedIndex--;
                            ActiveParent.SelectedIndex = Mathf.Max(ActiveParent.SelectedIndex, 0);
                            scrollToSelected = true;
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.DownArrow:
                        {
                            ActiveParent.SelectedIndex++;
                            ActiveParent.SelectedIndex = Mathf.Min(ActiveParent.SelectedIndex, ActiveChildren.Length - 1);
                            scrollToSelected = true;
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.RightArrow:
                        if (searchTree.SearchKeyIsEmpty && ActiveSearchEntry != null)
                        {
                            SelectEntry(ActiveSearchEntry);
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.LeftArrow:
                        if (searchTree.SearchKeyIsEmpty)
                        {
                            GoToParent();
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.KeypadEnter:
                        if (ActiveSearchEntry != null)
                        {
                            SelectEntry(ActiveSearchEntry);
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.Return:
                        if (ActiveSearchEntry != null)
                        {
                            SelectEntry(ActiveSearchEntry);
                            curentEvent.Use();
                        }
                        return;
                    case KeyCode.Backspace:
                        if (searchTree.SearchKeyIsEmpty)
                        {
                            GoToParent();
                            curentEvent.Use();
                        }
                        return;
                }
            }
        }
    }
}