using System.Collections.Generic;
using UnityEngine;

public class LocalizationData : ScriptableObject
{
    public List<LanguageData> Languages = new List<LanguageData>();

    public List<string> LanguageNames
    {
        get
        {
            List<string> languageNames = new List<string>();
            foreach (var language in Languages)
            {
                languageNames.Add(language.Name);
            }
            return languageNames;
        }
    }

    public Dictionary<string, string> GetLanguageData(string languageName)
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var language in Languages)
        {
            if (language.Name.Equals(languageName))
            {
                foreach(var word in language.Resources)
                {
                    dictionary.Add(word.Tag, word.StringData);
                }
                return dictionary;
            }
        }
        return null;
    }
}
