using UnityEditorInternal;

namespace ResourceLocalization
{
    public partial class TypesListView : ReorderableList
    {
        private ITypeLocalizationProvider provider;

        public TypesListView(ITypeLocalizationProvider provider) : base(provider.GetTypes(), typeof(TypeLocalization), true, true, true, true)
        {
            this.provider = provider ?? throw new System.ArgumentNullException(nameof(provider));
            onAddCallback = AddTypeComponent;
        }

        private void AddTypeComponent(ReorderableList reorderable)
        {
            AddTypeLocalizationWindow.Show(() => { });
        }
    }
}