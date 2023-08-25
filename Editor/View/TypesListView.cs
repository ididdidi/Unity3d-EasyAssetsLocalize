using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EasyAssetsLocalize
{
    public class TypesListView : ReorderableList
    {
        public TypesListView(LocalizationStorage storage) : base(null, typeof(System.Type), false, true, true, true)
        {
            list = new List<object>(from l in storage.Localizations.Where(i => i.IsDefault) select l.Type);
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
            if(list[index] is System.Type type)
            {
                EditorGUI.LabelField(position, type.GetContent());
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
            if(reorderable.list[reorderable.index] is System.Type type)
            {
                if (EditorExtends.DeleteConfirmation(type.Name))
                {
                    LocalizationManager.Storage.RemoveAll(type);
                    LocalizationBuilder.Remove(type);
                }
            }
            else { reorderable.list.RemoveAt(reorderable.index--); }
        }
    }
}