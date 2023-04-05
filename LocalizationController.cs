using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    public class LocalizationController : MonoBehaviour
    {
        [SerializeField] private LocalizationData defaultLocalization;
        private Dictionary<string, Dictionary<string, Dictionary<string, Data>>> localizations = new Dictionary<string, Dictionary<string, Dictionary<string, Data>>>();

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

        void Start()
        {
            AddLocalization("Default", defaultLocalization);
        }

        public void AddLocalization(string localizationName, LocalizationData localizationData)
        {
            var localization = new Dictionary<string, Dictionary<string, Data>>();

            if (localizations.ContainsKey(localizationName))
            {
                throw new Exception(string.Format("Localization with the name: {0}, has already been added.", localizationName));
            }

            foreach (var language in localizationData.Languages)
            {
                Dictionary<string, Data> resources;
                if (!localization.TryGetValue(language.Name, out resources))
                {
                    resources = new Dictionary<string, Data>();
                    localization.Add(language.Name, resources);
                }
                foreach (var resource in language.Resources)
                {
                    resources.Add(resource.Tag, resource.Data);
                }
            }

            if (localization.Count > 0)
            {
                localizations.Add(localizationName, localization);
                UptateLocalizations();
            }
            else
            {
                Debug.LogWarning(string.Format("{0} is empty.", localizationName));
            }
        }

        public void RemoveLocalization(string localizationName)
        {
            localizations.Remove(localizationName);
            UptateLocalizations();
        }

        public void SetLanguage(string language)
        {
            if (Languages.Contains(language)) { Language = language; }
            UptateLocalizations();
        }

        public Data GetLocalization(string tag)
        {
            var languagePacks = GetLanguagePacks();
            for (int i = languagePacks.Count - 1; i >= 0; i--)
            {
                if (languagePacks[i].ContainsKey(tag))
                {
                    return languagePacks[i][tag];
                }
            }
            return default;
        }

        public void UptateLocalizations()
        {
            var textViews = FindObjectsOfType<Text>();
            foreach (var textView in textViews)
            {
                var localisation = GetLocalization(textView.name);
                //if (!string.IsNullOrWhiteSpace(localisation)) { textView.text = localisation; }
            }
        }

        private List<Dictionary<string, Data>> GetLanguagePacks()
        {
            var languagePacks = new List<Dictionary<string, Data>>();
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
            UptateLocalizations();
        }

        public class Exception : System.Exception
        {
            public Exception(string message) : base(message) { }
        }
    }
}