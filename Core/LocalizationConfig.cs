using System.Collections.Generic;
using UnityEngine;

namespace ResourceLocalization
{
    public class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private List<Language> languages = new List<Language>();
        [SerializeField] private List<string> Types = new List<string>();
    }
}
