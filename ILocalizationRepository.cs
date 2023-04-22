
using System.Collections.Generic;

namespace ResourceLocalization
{
    public interface ILocalizationRepository
    {
        Language[] Languages { get; }

        Localization[] Localizations { get; }

        void AddLanguage(string name);

        void RemoveLanguage(string name);

        bool Conteins(LocalizationTag tag);

        void AddResource(LocalizationTag tag, Resource resource);

        void InsertResource(int index, Localization localization);

        void RemoveResource(int index);

        void RemoveResource(LocalizationTag tag);

        Dictionary<LocalizationTag, Resource> GetLocalization(string language);
    }
}
