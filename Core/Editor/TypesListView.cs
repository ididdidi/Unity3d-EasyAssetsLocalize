using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using EasyLocalization;

namespace EasyLocalization
{
    public partial class TypesListView : ReorderableList
    {
        public TypesListView() : base(new List<object>(TypeMetadata.GetAllMetadata()), typeof(TypeMetadata), false, true, true, true)
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
            if(list[index] is TypeMetadata meta)
            {
                EditorGUI.LabelField(position, new GUIContent(meta.Type.Name, meta.Icon, meta.Type.ToString()));
            }
            else
            {
                NewValueField(position, index);
            }
        }

        private void NewValueField(Rect position, int index)
        {
            EditorGUI.BeginChangeCheck();
            list[index] = EditorGUI.ObjectField(position, (Object)list[index], typeof(Object), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (list[index] != null && LocalizationBuilder.Conteins(list[index].GetType())) { 
                    LocalizationBuilder.CreateComponent(LocalizationManager.Storage, list[index]);
                }
            }
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            reorderable.list.Add(null);
            reorderable.index = list.Count - 1;
        }

        private void RemoveTypeComponent(ReorderableList reorderable)
        {
            if(reorderable.list[reorderable.index] is TypeMetadata metadata)
            {
                if (ExtendedEditor.DeleteConfirmation(metadata.Type.Name))
                {
                    LocalizationManager.Storage.RemoveAll(metadata.Type);
                    LocalizationBuilder.Remove(metadata.Type);
                }
            }
            else { reorderable.list.RemoveAt(reorderable.index--); }
        }
    }
}