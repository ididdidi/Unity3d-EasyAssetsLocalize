using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
    public partial class TypesListView : ReorderableList
    {
        public TypesListView() : base(TypesMetaProvider.GetTypesMeta(), typeof(TypeMetadata), false, true, true, true)
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
            var meta = (TypeMetadata)list[index];
            EditorGUI.LabelField(position, new GUIContent(meta.Type.Name, meta.Texture));
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            AddTypeLocalizationWindow.Show();
            AssetDatabase.Refresh();
        }

        private void RemoveTypeComponent(ReorderableList reorderable)
        {
            TypesMetaProvider.PemoveType((TypeMetadata)reorderable.list[reorderable.index]);
            AssetDatabase.Refresh();
        }
    }
}