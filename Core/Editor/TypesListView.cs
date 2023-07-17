using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ResourceLocalization
{
    public partial class TypesListView : ReorderableList
    {
        private TypesMetaProvider provider;

        public TypesListView(TypesMetaProvider provider) : base(provider.GetTypesMeta(), typeof(TypeMetadata), false, true, true, true)
        {
            this.provider = provider ?? throw new System.ArgumentNullException(nameof(provider));
            drawElementCallback = DrawTypeMeta;
            onAddCallback = AddTypeComponent;
        }

        private void DrawTypeMeta(Rect rect, int index, bool isActive, bool isFocused)
        {
            var meta = (TypeMetadata)list[index];
            EditorGUI.LabelField(rect, new GUIContent(meta.Type.Name, meta.Texture));
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            AddTypeLocalizationWindow.Show((typeName) => {
                var metadata = provider.GetTypesMeta();
                reorderable.list = metadata;
                for(int i=0; i < metadata.Length; i++)
                {
                    if (metadata[i].Type.Name.Equals(typeName)) { reorderable.index = i; return; }
                }
            });
        }
    }
}