namespace ResourceLocalization
{
    public class LocalizationCreater
    {
        public LocalizationTag Create(string name, IResource resource, int quantity)
        {
            var localizationTag = new LocalizationTag(name, System.Linq.Enumerable.Repeat(resource.Clone(), quantity));
            return localizationTag;
        }
    }
}