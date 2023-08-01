using SimpleLocalization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExtended;

namespace SimpleLocalization
{
    public class LocalizationSearchProvider : ISearchTreeProvider
    {
        private SearchTree searchTree = new SearchTree();

        public delegate bool Handler(object data);
        private Handler handler;

        public delegate void OnFocus(object data);
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

                if (!entries.ContainsKey(tags[i].Type)) { entries.Add(tags[i].Type, new List<SearchTreeEntry>()); }
                entries[tags[i].Type].Add(new SearchTreeEntry(content, Type == null ? 2 : 1, tags[i]));
            }

            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            foreach (var entry in entries)
            {
                if (Type == null)
                {
                    var cover = new GUIContent(entry.Key.Name, entry.Value[0].Content.image);
                    searchList.Add(new SearchTreeGroupEntry(new GUIContent(entry.Key.Name), 1, cover));
                }
                searchList.AddRange(entry.Value);
            }

            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry entry) => (bool)handler?.Invoke(entry.Data);

        public void OnFocusEntry(SearchTreeEntry entry)
        {
            var content = new GUIContent("Not found...", EditorGUIUtility.IconContent("Search Icon").image);
            onFocus?.Invoke(entry?.Data ?? content);
        }

        private List<SearchTreeEntry> GetNewItems()
        {
            TypeMetadata[] metaDatas = TypeMetadata.GetAllMetadata();
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();

            if (Type == null)
            {
                var cover = new GUIContent("Add New", EditorGUIUtility.IconContent("CreateAddNew@2x").image);
                searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1, cover));
            }

            var icon = EditorGUIUtility.IconContent("CreateAddNew").image;
            var level = 1;
            for (int i = 0; i < metaDatas.Length; i++)
            {
                if (Type != null)
                {
                    if (!Type.Equals(metaDatas[i].Type)) { continue; }
                }
                else
                {
                    icon = metaDatas[i].Icon; level = 2;
                }
                var defValue = LocalizationManager.Storage.GetDefaultLocalization(metaDatas[i].Type);
                searchList.Add(new SearchTreeEntry(new GUIContent($"New {defValue.Type.Name} Localization", icon), level, defValue.Clone()));
            }
            return searchList;
        }
    }
}