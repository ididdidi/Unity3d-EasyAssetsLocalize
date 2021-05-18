using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationController : MonoBehaviour
{
    [SerializeField] private LocalizationData defaultLocalization;
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localizations = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    public string Language
    {
        get => (PlayerPrefs.HasKey("Language")) ? PlayerPrefs.GetString("Language") : Application.systemLanguage.ToString();
        set => PlayerPrefs.SetString("Language", value);
    }

    public List<string> Languages
    {
        get { 
            var languages = new List<string>();
            foreach(var local in localizations)
            {
                foreach(var language in local.Value.Keys)
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
        var localization = new Dictionary<string, Dictionary<string, string>>();

        if (localizations.ContainsKey(localizationName)) { 
            throw new Exception(string.Format("Localization with the name: {0}, has already been added.", localizationName));
        }

        foreach (var language in localizationData.Languages)
        {
            Dictionary<string, string> words;
            if (!localization.TryGetValue(language.Name, out words))
            {
                words = new Dictionary<string, string>();
                localization.Add(language.Name, words);
            }
            foreach (var word in language.Resources)
            {
                words.Add(word.Tag, word.StringData);
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
        if(Languages.Contains(language)) { Language = language; }
        UptateLocalizations();
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

    public void UptateLocalizations()
    {
        var textViews = FindObjectsOfType<Text>();
        foreach (var textView in textViews)
        {
            var localisation = GetLocalization(textView.name);
            if (!string.IsNullOrWhiteSpace(localisation)){ textView.text = localisation; }
        }
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
        UptateLocalizations();
    }

    public class Exception : System.Exception
    {
        public Exception (string message) : base(message) { }
    }
}
