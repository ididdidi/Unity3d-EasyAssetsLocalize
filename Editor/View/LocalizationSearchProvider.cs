using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Search Localization Data Provider.
    /// </summary>
    public class LocalizationSearchProvider : ISearchTreeProvider
    {
        private SearchTree searchTree = new SearchTree();
        private LocalizationStorage Storage { get; }
        private System.Type Type { get; }
        private Dictionary<System.Type, List<Localization>> types;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="storage">Localization storage</param>
        /// <param name="onSelect">On select Callback</param>
        /// <param name="onFocus">On focus Callback</param>
        /// <param name="type">Target type</param>
        public LocalizationSearchProvider(LocalizationStorage storage, System.Type type = null)
        {
            Storage = storage;
            Type = type;
        }

        /// <summary>
        /// Creates and returns a search tree.
        /// </summary>
        /// <returns><see cref="SearchTree"/> as a list of localizations</returns>
        public SearchTree GetSearchTree()
        {
            if (!Storage) { throw new System.ArgumentNullException(nameof(Component)); }

            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("Localizations")));
            var tags = new List<Localization>(Storage.Localizations);
            tags.Sort((tag0, tag1) => tag0.Name.CompareTo(tag1.Name));

            searchList.AddRange(Adapt(tags));

            searchList.AddRange(GetNewItems());
            searchTree.BuildStack(searchList.ToArray());

            return searchTree;
        }

        /// <summary>
        /// Creates a list of <see cref="SearchTreeEntry"/> from list of <see cref="Localization"/>
        /// </summary>
        /// <param name="tags"></param>
        /// <returns>List of localizations as SearchTreeEntry</returns>
        private List<SearchTreeEntry> Adapt(List<Localization> tags)
        {
            // Group by Type
            Dictionary<System.Type, List<SearchTreeEntry>> entries = new Dictionary<System.Type, List<SearchTreeEntry>>();

            for (int i = 0; i < tags.Count; i++)
            {
                // If the target type is specified and does not match the type for item, then skip this iteration.
                if (Type != null && !Type.IsAssignableFrom(tags[i].Type)) { continue; }

                // Creating Content to show on the List
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

                // If the list for this type has not yet been added to the dictionary, then we create it and add.
                if (!entries.ContainsKey(tags[i].Type)) { entries.Add(tags[i].Type, new List<SearchTreeEntry>()); }
                // Adding the created item to the dictionary
                entries[tags[i].Type].Add(new SearchTreeEntry(content, Type == null ? 2 : 1, tags[i]));
            }

            // Create a list with tree leaves
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            foreach (var entry in entries)
            {
                // If the target type is not set, create a group for the item type.
                if (Type == null)
                {
                    var cover = new GUIContent(entry.Key.Name, entry.Value[0].Content.image);
                    searchList.Add(new SearchTreeGroupEntry(new GUIContent(entry.Key.Name), 1, cover));
                }
                // Adding items to the end of the list
                searchList.AddRange(entry.Value);
            }

            return searchList;
        }

        /// <summary>
        /// Creates new elements for the list for a specific type or for all.
        /// </summary>
        /// <returns>List of new localizations as SearchTreeEntry</returns>
        private List<SearchTreeEntry> GetNewItems()
        {
            TypeMetadata[] metaDatas = TypeMetadata.GetAllMetadata();
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();

            // If the target type is not initialized, then create a group for new localizations.
            if (Type == null)
            {
                var cover = new GUIContent("Add New", EditorGUIUtility.IconContent("CreateAddNew@2x").image);
                searchList.Add(new SearchTreeGroupEntry(new GUIContent("New Localization"), 1, cover));
            }

            var icon = EditorGUIUtility.IconContent("CreateAddNew").image;
            var level = 1;

            // Loop through all types of resources for localization.
            for (int i = 0; i < metaDatas.Length; i++)
            {
                // If the target type is defined.
                if (Type != null)
                {
                    // But this type does not correspond to it - we skip the iteration.
                    if (!Type.Equals(metaDatas[i].Type)) { continue; }
                }
                else
                {
                    // Initialize the icon according to the resource type and specify the level as 2.
                    icon = metaDatas[i].Icon; level = 2;
                }

                // Find the default value for this type, create a new element and add it to the list of localizations.
                var defValue = LocalizationManager.Storage.GetDefaultLocalization(metaDatas[i].Type);
                searchList.Add(new SearchTreeEntry(new GUIContent($"New {defValue.Type.Name} Localization", icon), level, defValue.Clone()));
            }
            return searchList;
        }
    }
}