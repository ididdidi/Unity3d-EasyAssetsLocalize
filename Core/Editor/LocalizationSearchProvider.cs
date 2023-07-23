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
    private Dictionary<System.Type, List<LocalizationTag>> types;

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

        searchList.AddRange(BuildSearchList(tags));

        searchList.AddRange(GetNewItems());
        searchTree.BuildStack(searchList.ToArray());

        return searchTree;
    }

    private List<SearchTreeEntry> BuildSearchList(List<LocalizationTag> tags)
    {
        // Group by Type
        Dictionary<System.Type, List<SearchTreeEntry>> entries = new Dictionary<System.Type, List<SearchTreeEntry>>();

        for (int i = 0; i < tags.Count; i++)
        {
            if (Type != null && !Type.IsAssignableFrom(tags[i].Type)) { continue; }

            GUIContent content;
            if (typeof(string).IsAssignableFrom(tags[i].Type))
            {
                content = new GUIContent(tags[i].Name, EditorGUIUtility.IconContent("Text Icon").image);
            }
            else
            {
                Texture icon;
                if (typeof(ScriptableObject).IsAssignableFrom(tags[i].Type)) { icon = EditorGUIUtility.IconContent("ScriptableObject Icon").image; }
                else { icon = EditorGUIUtility.ObjectContent(null, tags[i].Type).image; }
                content = new GUIContent(tags[i].Name, icon);
            }

            if (!entries.ContainsKey(tags[i].Type)){ entries.Add(tags[i].Type, new List<SearchTreeEntry>()); }
            entries[tags[i].Type].Add(new SearchTreeEntry(content, Type == null? 2 : 1, tags[i]));
        }

        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        foreach (var entry in entries)
        {
            if (Type == null) { searchList.Add(new SearchTreeGroupEntry(new GUIContent(entry.Key.Name), 1)); }
            searchList.AddRange(entry.Value);
        }

        return searchList;
    }

    public bool OnSelectEntry(SearchTreeEntry entry) => (bool)handler?.Invoke(entry.Data as LocalizationTag);

    public void OnFocusEntry(SearchTreeEntry entry) => onFocus?.Invoke(entry.Data as LocalizationTag);

    private List<SearchTreeEntry> GetNewItems()
    {
        TypeMetadata[] metaData = TypesMetaProvider.GetTypesMeta();
        List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
        if (Type != null)
        {
            searchList.Add(NewItem(Type));
        }
        else
        {
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1));
            for (int i=0; i < metaData.Length; i++)
            {
                searchList.Add(NewItem(metaData[i].Type, 2));
            }
        }
        return searchList;
    }

    private SearchTreeEntry NewItem(System.Type type, int level = 1)
    {
        Texture icon;
        var resources = new IResource[LocalizationManager.Languages.Length];
        if (typeof(string).IsAssignableFrom(type))
        {
            icon = EditorGUIUtility.IconContent("Text Icon").image;
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = new TextResource("Text");
            }
        }
        else
        {
            if (typeof(ScriptableObject).IsAssignableFrom(type)) { icon = EditorGUIUtility.IconContent("ScriptableObject Icon").image; }
            else { icon = EditorGUIUtility.ObjectContent(null, type).image; }
            for (int i = 0; i < resources.Length; i++)
            {
                resources[i] = new UnityResource(null);
            }
        }

        return new SearchTreeEntry(new GUIContent($"New {type.Name} Localization", icon), level, new LocalizationTag(type, resources));
    }
}