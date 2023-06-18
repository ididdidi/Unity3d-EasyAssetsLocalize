using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public LocalizationComponent Component { get; set; }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if(!Component) { throw new System.ArgumentNullException(nameof(Component)); }

            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("Localizations")));
            var tags = new List<LocalizationTag>(Component.Storage.LocalizationTags);
            tags.Sort((tag0, tag1) => tag0.Name.CompareTo(tag1.Name));

            GUIContent content;
            for (int i = 0; i < tags.Count; i++)
            {
                var data = tags[i].Resources[0].Data;
                if (!Component.Type.IsAssignableFrom(tags[i].Type)) { continue; }
                else if (tags[i].Type.IsAssignableFrom(typeof(string)))
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
            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Component.Tag = SearchTreeEntry.userData as LocalizationTag;
            return true;
        }
    }
}
