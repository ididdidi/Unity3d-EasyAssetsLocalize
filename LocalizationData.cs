using System.Collections.Generic;
using UnityEngine;

namespace ru.mofrison.Unity3D.Localization
{
    public class LocalizationData : ScriptableObject
    {
        public List<LanguageData> Languages = new List<LanguageData>();

        public List<string> LanguageNames
        {
            get
            {
                List<string> languageNames = new List<string>();
                for (int i=0; i < Languages.Count; i++)
                {
                    languageNames.Add(Languages[i].Name);
                }
                return languageNames;
            }
        }

        public Dictionary<string, string> GetLanguageData(string languageName)
        {
            var dictionary = new Dictionary<string, string>();
            for (int i=0; i < Languages.Count; i++)
            {
                if (Languages[i].Name.Equals(languageName))
                {
                    for (int j=0; j < Languages[i].Resources.Count; j++)
                    {
                        dictionary.Add(Languages[i].Resources[j].Tag, Languages[i].Resources[j].StringData);
                    }
                    return dictionary;
                }
            }
            return null;
        }
    }
}
