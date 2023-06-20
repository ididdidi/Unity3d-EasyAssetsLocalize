using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SearchTreeEntry : System.IComparable<SearchTreeEntry>
{
    public int level;
    public GUIContent content;

    public object userData;

    public SearchTreeEntry(GUIContent content)
    {
        this.content = content;
    }

    public string name
    {
        get { return content.text; }
    }

    public int CompareTo(SearchTreeEntry o)
    {
        return name.CompareTo(o.name);
    }

    public List<SearchTreeEntry> GetChildren(SearchTreeEntry[] tree, bool hasSearch)
    {
        List<SearchTreeEntry> children = new List<SearchTreeEntry>();
        int level = -1;
        int i = 0;
        for (i = 0; i < tree.Length; i++)
        {
            if (tree[i] == this)
            {
                level = this.level + 1;
                i++;
                break;
            }
        }
        if (level == -1)
            return children;

        for (; i < tree.Length; i++)
        {
            SearchTreeEntry e = tree[i];

            if (e.level < level)
                break;
            if (e.level > level && !hasSearch)
                continue;

            children.Add(e);
        }

        return children;
    }
}

[System.Serializable]
public class SearchTreeGroupEntry : SearchTreeEntry
{
    public int selectedIndex;
    public Vector2 scroll;

    public SearchTreeGroupEntry(GUIContent content, int level = 0)
        : base(content)
    {
        this.content = content;
        this.level = level;
    }
}
