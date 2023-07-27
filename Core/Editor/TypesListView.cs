using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
    public partial class TypesListView : ReorderableList
    {
        public TypesListView() : base(new List<TypeMetadata>(TypeMetadata.GetAllMetadata()), typeof(TypeMetadata), false, true, true, true)
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
                var width = position.width;
                position.width = 143;
                EditorGUI.LabelField(position, new GUIContent(meta.Type.Name, meta.Icon, meta.Type.ToString()));
                
                position.x += position.width;
                position.height -= 3;
                position.width = width - position.width;

                NewValueField(position, meta);
            }
        }

        private void NewValueField(Rect position, TypeMetadata metadata)
        {
            object @object; 
            EditorGUI.BeginChangeCheck();
            if (typeof(string).IsAssignableFrom(metadata.Type))
            {
                @object = EditorGUI.TextField(position, (string)metadata.Default);
            }
            else
            {
                @object = EditorGUI.ObjectField(position, (Object)metadata.Default, metadata.Type, false);
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (@object != null && !@object.Equals(metadata.Default))
                {
                    metadata.Default = @object;
                }
            }
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            reorderable.list.Add(new TypeMetadata(typeof(Object), null));
            reorderable.index = list.Count - 1;
        }

        private void RemoveTypeComponent(ReorderableList reorderable)
        {
            if(reorderable.list[reorderable.index] is TypeMetadata metadata) {
                if (metadata.Default != null) { LocalizationBuilder.Remove(metadata.Type); }
                else { reorderable.list.RemoveAt(reorderable.index--); }
            }
        }
    }
}