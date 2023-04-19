using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationStorage localizationStorage;
        [SerializeField] private List<LocalizationReceiver> receivers;
        private Dictionary<string, Resource> dictionary = new Dictionary<string, Resource>();

        public string Language
        {
            get => (PlayerPrefs.HasKey("Language")) ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString();
            set => PlayerPrefs.SetString("Language", value);
        }

        public List<string> Languages
        {
            get
            {
                var languages = new List<string>(localizationStorage.Languages.Length);
                for(int i=0; i < languages.Count; i++)
                {
                    languages[i] = localizationStorage.Languages[i].Name;
                }
                return languages;
            } 
            
        }

        public List<LocalizationReceiver> Receivers { get => receivers; }
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

        public void SetLanguage(string language)
        {
            Language = language;
         //   dictionary = localizationStorage.GetLocalization(language).Dictionary;
            
            foreach (LocalizationReceiver receiver in receivers)
            {
                receiver.Object.Data = dictionary[receiver.ID];
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