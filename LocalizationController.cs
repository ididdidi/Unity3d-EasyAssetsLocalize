using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationStorage localizationStorage;
        [SerializeField, HideInInspector] private List<LocalizationReceiver> localizationreceivers;
        private Dictionary<string, Resource> dictionary;

        public Language Language
        {
            get => new Language(PlayerPrefs.HasKey("Language") ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString());
            set => PlayerPrefs.SetString("Language", value.Name);
        }
        public List<LocalizationReceiver> Receivers { get => localizationreceivers; }
        public LocalizationStorage LocalizationStorage { get => localizationStorage; }

        void Start()
        {
            LoadLocalization(localizationStorage);
        }

        public void LoadLocalization(LocalizationStorage localizationStorage)
        {
            this.localizationStorage = localizationStorage;
            SetLanguage(Language);
        }

        public void SetLanguage(Language language)
        {
            dictionary = localizationStorage.GetDictionary(language);
            Language = language;

            foreach (LocalizationReceiver receiver in localizationreceivers)
            {
                receiver.Resource = dictionary[receiver.ID];
            }
        }

        public void SetNextLanguage()
        {
            ChangeLocalzation(Direction.Next);
        }

        public void SetPrevLanguage()
        {
            ChangeLocalzation(Direction.Back);
        }

        private enum Direction { Next = 1, Back =-1 }

        private void ChangeLocalzation(Direction direction)
        {
            var languages = new List<Language>(localizationStorage.Languages);
            if (languages.Count < 2) { return; }

            int index = languages.IndexOf(Language);
            if (index > -1)
            {
                int newIndex = (index + (int)direction) % languages.Count;
                if (newIndex >= 0) { index = newIndex; }
                else { index = languages.Count + newIndex; }
                SetLanguage(languages[index]);
            }
            else
            {
                throw new System.ArgumentException($"{Language} not found in the {localizationStorage}");
            }
        }
    }
}