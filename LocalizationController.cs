using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationStorage localizationStorage;
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public string Language
        {
            get => (PlayerPrefs.HasKey("Language")) ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString();
            set => PlayerPrefs.SetString("Language", value);
        }

        public List<string> Languages { get; private set; }

        void Start()
        {
            LoadLocalization(localizationStorage);
        }

        public void LoadLocalization(LocalizationStorage localization)
        {
            Languages = localization.Languages;
            SetLanguage(Language);
        }

        public void SetLanguage(string language)
        {
            if (Languages.Contains(language))
            {
                dictionary = localizationStorage.GetLocalization(Language).Dictionary;
            }
            else
            {
                throw new System.ArgumentException($"{Language} not found in the {Languages}");
            }
            UptateLocalization();
        }

        public void UptateLocalization()
        {
            var textViews = FindObjectsOfType<Text>();
            foreach (var textView in textViews)
            {
                var localisation = dictionary[textView.name] as string;
                if (!string.IsNullOrWhiteSpace(localisation)) { textView.text = localisation; }
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
            if (Languages.Count < 2) { return; }
            int index = Languages.IndexOf(Language);
            if (index > -1)
            {
                int newIndex = (index + (int)direction) % Languages.Count;
                if (newIndex >= 0) { index = newIndex; }
                else { index = Languages.Count + newIndex; }
                SetLanguage(Languages[index]);
            }
            else
            {
                throw new System.ArgumentException($"{Language} not found in the {Languages}");
            }
        }
    }
}