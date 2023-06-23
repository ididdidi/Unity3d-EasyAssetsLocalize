﻿using ResourceLocalization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExtended;

public class LocalizationSearchProvider : ISearchTreeProvider
{
    private LocalizationComponent Component { get; }

    public LocalizationSearchProvider(LocalizationComponent component) => Component = component;

    public SearchTreeEntry[] CreateSearchTree()
    {
        if (!Component) { throw new System.ArgumentNullException(nameof(Component)); }

        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Localizations")));
        var tags = new List<LocalizationTag>(Component.Storage.LocalizationTags);
        tags.Sort((tag0, tag1) => tag0.Name.CompareTo(tag1.Name));

        GUIContent content;
        for (int j = 0; j < 1000; j++)
            for (int i = 0; i < tags.Count; i++)
            {
                var data = tags[i].Resources[0].Data;
                if (!Component.Type.IsAssignableFrom(tags[i].Type)) { continue; }
                if (tags[i].Type.IsAssignableFrom(typeof(string)))
                {
                    content = new GUIContent(tags[i].Name, EditorGUIUtility.IconContent("Text Icon").image);
                }
                else
                {
                    content = new GUIContent(tags[i].Name, EditorGUIUtility.ObjectContent((Object)data, data.GetType()).image);
                }

                var item = new SearchTreeEntry(content, 1, tags[i]);
                searchList.Add(item);
            }

        searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1));

        GetNewItems(searchList);

        return searchList.ToArray();
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry)
    {
        Component.Tag = SearchTreeEntry.Data as LocalizationTag;
        return true;
    }

    private void GetNewItems(List<SearchTreeEntry> searchList)
    {
        for (int i = 0; i < 4; i++)
        {
            //  if (!Component.Type.IsAssignableFrom(tags[i].Type)) { continue; }
            var content = new GUIContent("New Text", EditorGUIUtility.IconContent("Text Icon").image);
            var data = new LocalizationTag("New Text", new TextResource("Text"));
            searchList.Add(new SearchTreeEntry(content, 2, data));
        }
    }
}