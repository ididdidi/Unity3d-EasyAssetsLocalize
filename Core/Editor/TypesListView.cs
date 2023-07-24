using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
    public partial class TypesListView : ReorderableList
    {
        private Object newObject;

        public TypesListView() : base(new List<TypeMetadata>(TypesMetaProvider.GetTypesMeta()), typeof(TypeMetadata), false, true, true, true)
        {
            drawHeaderCallback = DrawHeader;
            drawElementCallback = DrawTypeMeta;
            onAddCallback = AddTypeComponent;
            onRemoveCallback = RemoveTypeComponent;
        }

        private void DrawHeader(Rect position)
        {
            EditorGUI.LabelField(position, "Types");
        }

        private void DrawTypeMeta(Rect position, int index, bool isActive, bool isFocused)
        {
            if( list[index] is TypeMetadata meta)
            {
                EditorGUI.LabelField(position, new GUIContent(meta.Type.Name, meta.Texture, meta.Type.ToString()));
            }
            else
            {
                NewObjectTypeField(position);
            }
        }

        private void NewObjectTypeField(Rect position)
        {
            EditorGUI.BeginChangeCheck();
            newObject = EditorGUI.ObjectField(position, newObject, typeof(Object), false);
            if (EditorGUI.EndChangeCheck())
            {
                TypesMetaProvider.AddType(newObject.GetType());
                AssetDatabase.Refresh();
            }
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
            reorderable.index = list.Count - 1;
        }

        private void RemoveTypeComponent(ReorderableList reorderable)
        {
            if(reorderable.list[reorderable.index] is TypeMetadata metadata) { TypesMetaProvider.RemoveType(metadata); }
            else { reorderable.list.RemoveAt(reorderable.index); reorderable.index--; }
        }
    }
}