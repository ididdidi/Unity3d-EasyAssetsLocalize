namespace ResourceLocalization
{
    public interface ITypeLocalizationProvider
    {
        TypeLocalization[] GetTypes();
        void AddType(TypeLocalization newType);
    }
}