
using System.Collections.Generic;

namespace ResourceLocalization
{
    public interface ILocalizationRepository
    {
        Language[] Languages { get; }

        Localization[] Localizations { get; }

        void AddLanguage(string name);

        void RemoveLanguage(string name);

        bool Conteins(Tag tag);

        void AddResource(Tag tag, Resource resource);

        void InsertResource(int index, Localization localization);

        void RemoveResource(int index);

        void RemoveResource(Tag tag);

        Dictionary<Tag, Resource> GetLocalization(string language);
    }
}
