using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ru.mofrison.Unity3d
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationData defaultLocalization;
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localizations = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        public delegate void ChangeLanguage();
        private ChangeLanguage changeLanguage;

        public string Language
        {
            get => (PlayerPrefs.HasKey("Language")) ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString();
            set => PlayerPrefs.SetString("Language", value);
        }

        public List<string> Languages
        {
            get
            {
                var languages = new List<string>();
                foreach (var local in localizations)
                {
                    foreach (var language in local.Value.Keys)
                    {
                        if (!languages.Contains(language))
                        {
                            languages.Add(language);
                        }
                    }
                }
                return languages;
            }
        }

        void Awake()
        {
            AddLocalization("Default", defaultLocalization);
        }

        public void AddLocalization(string localizationName, LocalizationData localizationData)
        {
            if (!localizationData) { return; }
            var localization = new Dictionary<string, Dictionary<string, string>>();

            if (localizations.ContainsKey(localizationName))
            {
                throw new Exception(string.Format("Localization with the name: {0}, has already been added.", localizationName));
            }

            for (int i=0; i < localizationData.Languages.Count; i++)
            {
                Dictionary<string, string> words;
                if (!localization.TryGetValue(localizationData.Languages[i].Name, out words))
                {
                    words = new Dictionary<string, string>();
                    localization.Add(localizationData.Languages[i].Name, words);
                }
                for (int j=0; j < localizationData.Languages[i].Resources.Count; j++)
                {
                    words.Add(localizationData.Languages[i].Resources[j].Tag, localizationData.Languages[i].Resources[j].StringData);
                }
            }

            if (localization.Count > 0)
            {
                localizations.Add(localizationName, localization);
                UpdateLocalizations();
            }
            else
            {
                Debug.LogWarning(string.Format("{0} is empty.", localizationName));
            }
        }

        public void RemoveLocalization(string localizationName)
        {
            if (localizations.ContainsKey(localizationName)) { localizations.Remove(localizationName); }
            UpdateLocalizations();
        }

        public void SetLanguage(string language)
        {
            if (Languages.Contains(language)) { Language = language; }
            UpdateLocalizations();
        }

        public string GetLocalization(string tag)
        {
            var languagePacks = GetLanguagePacks();
            for (int i = languagePacks.Count - 1; i >= 0; i--)
            {
                if (languagePacks[i].ContainsKey(tag))
                {
                    return languagePacks[i][tag];
                }
            }
            return null;
        }

        public void UpdateLocalizations()
        {
            var textViews = FindObjectsOfType<Text>();
            for (int i=0; i < textViews.Length; i++)
            {
                var localisation = GetLocalization(textViews[i].name);
                if (!string.IsNullOrWhiteSpace(localisation)) { textViews[i].text = localisation; }
            }

            changeLanguage?.Invoke();
        }

        private List<Dictionary<string, string>> GetLanguagePacks()
        {
            var languagePacks = new List<Dictionary<string, string>>();
            foreach (var localization in localizations)
            {
                if (localization.Value.ContainsKey(Language))
                {
                    languagePacks.Add(localization.Value[Language]);
                }
                else
                {
                    Debug.LogWarning(string.Format("{0} not found in the {1}", Language, localization.Key));
                }
            }
            return languagePacks;
        }

        public void ChangeLocalzation(int offset)
        {
            if (offset == 0) return;
            int index = Languages.IndexOf(Language);
            int newIndex = (index + offset) % (Languages.Count);
            if (newIndex >= 0) { index = newIndex; }
            else
            {
                index = Languages.Count + newIndex;
            }
            Language = Languages[index];
            UpdateLocalizations();
        }

        public void AddChangeLanguageCallback(ChangeLanguage callback)
        {
            changeLanguage += callback;
        }
        public void RemoveChangeLanguageCallback(ChangeLanguage callback)
        {
            changeLanguage -= callback;
        }

        public class Exception : System.Exception
        {
            public Exception(string message) : base(message) { }
        }
    }
}
