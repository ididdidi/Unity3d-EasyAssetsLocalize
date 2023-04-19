
using System.Collections.Generic;

namespace ResourceLocalization
{
    public interface ILocalizationRepository
    {
        Language[] Languages { get; }

        Localization[] Localizations { get; }

        void AddLanguage(string name);

        void RemoveLanguage(string name);

        void AddResource(string name, Resource resource);

        void InsertResource(int index, Localization localization);

        void RemoveResource(int index);

        Dictionary<Tag, Resource> GetLocalization(string language);
    }
}
