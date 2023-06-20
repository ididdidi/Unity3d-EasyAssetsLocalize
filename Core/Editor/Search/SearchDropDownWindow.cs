using ResourceLocalization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SearchDropDownWindow : DropDownWindow<LocalizationComponent>, SearchTreeView.IContext
{
    private LocalizationComponent component;

    // Constants
    private const float defaultWidth = 240f;
    private const float defaultHeight = 320f;

    public override void Init(LocalizationComponent component)
    {
        this.component = component as LocalizationComponent;
        this.view = new SearchTreeView(this);
    }

    public SearchTreeEntry[] CreateSearchTree()
    {
        if (!component) { throw new System.ArgumentNullException(nameof(Component)); }

        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Localizations")));
        var tags = new List<LocalizationTag>(component.Storage.LocalizationTags);
        tags.Sort((tag0, tag1) => tag0.Name.CompareTo(tag1.Name));

        GUIContent content;
        for (int i = 0; i < tags.Count; i++)
        {
            var data = tags[i].Resources[0].Data;
            if (!component.Type.IsAssignableFrom(tags[i].Type)) { continue; }
            if (tags[i].Type.IsAssignableFrom(typeof(string)))
            {
                content = new GUIContent(tags[i].Name, EditorGUIUtility.IconContent("Text Icon").image);
            }
            else
            {
                content = new GUIContent(tags[i].Name, EditorGUIUtility.ObjectContent((Object)data, data.GetType()).image);
            }

            var item = new SearchTreeEntry(content);
            item.userData = tags[i];
            item.level = 1;
            searchList.Add(item);
        }

        searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1));
        var newitem = new SearchTreeEntry(new GUIContent("New", EditorGUIUtility.IconContent("Text Icon").image));
        newitem.level = 2;
        searchList.Add(newitem);
        return searchList.ToArray();
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry)
    {
        component.Tag = SearchTreeEntry.userData as LocalizationTag;
        return true; ;
    }
}
