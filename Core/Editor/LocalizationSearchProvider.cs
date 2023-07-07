using ResourceLocalization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExtended;

public class LocalizationSearchProvider : ISearchTreeProvider
{
    private SearchTree searchTree = new SearchTree();

    public delegate bool Handler(LocalizationTag tag);
    private Handler handler;

    public delegate void OnFocus(LocalizationTag tag);
    private OnFocus onFocus;

    private LocalizationStorage Storage { get; }
    private System.Type Type { get; }

    public LocalizationSearchProvider(LocalizationStorage storage, Handler handler, OnFocus onFocus = null, System.Type type = null)
    {
        Storage = storage;
        this.handler = handler;
        this.onFocus = onFocus;
        Type = type;
    }

    public SearchTree GetSearchTree()
    {
        if (!Storage) { throw new System.ArgumentNullException(nameof(Component)); }

        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Localizations")));
        var tags = new List<LocalizationTag>(Storage.LocalizationTags);
        tags.Sort((tag0, tag1) => tag0.Name.CompareTo(tag1.Name));

        GUIContent content;
        searchList.Add(new SearchTreeGroupEntry(new GUIContent("Text"), 1));
        for (int j = 0; j < 10; j++)
            for (int i = 0; i < tags.Count; i++)
            {
                var data = tags[i].Resources[0].Data;
                if (Type != null && !Type.IsAssignableFrom(tags[i].Type)) { continue; }
                if (tags[i].Type.IsAssignableFrom(typeof(string)))
                {
                    content = new GUIContent(tags[i].Name, EditorGUIUtility.IconContent("Text Icon").image);
                }
                else
                {
                    content = new GUIContent(tags[i].Name, EditorGUIUtility.ObjectContent((Object)data, data.GetType()).image);
                }

                var item = new SearchTreeEntry(content, 2, tags[i]);
                searchList.Add(item);
            }

        searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1));

        GetNewItems(searchList);
        searchTree.BuildStack(searchList.ToArray());

        return searchTree;
    }

    public bool OnSelectEntry(SearchTreeEntry entry) => (bool)handler?.Invoke(entry.Data as LocalizationTag);

    public void OnFocusEntry(SearchTreeEntry entry) => onFocus?.Invoke(entry.Data as LocalizationTag);

    private void GetNewItems(List<SearchTreeEntry> searchList)
    {
        for (int j = 0; j < 1; j++)
        {
            //  if (!Component.Type.IsAssignableFrom(tags[i].Type)) { continue; }
            var content = new GUIContent("New Text", EditorGUIUtility.IconContent("Text Icon").image);

            var resources = new IResource[Storage.Languages.Length];
            for (int i=0; i < resources.Length; i++)
            {
                resources[i] = new TextResource("Text");
            }

            var data = new LocalizationTag("New Text", resources);
            searchList.Add(new SearchTreeEntry(content, 2, data));
        }
    }
}