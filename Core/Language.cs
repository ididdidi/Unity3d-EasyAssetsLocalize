using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Class for storing language information.
    /// </summary>
    [System.Serializable]
    public class Language
    {
        [field: SerializeField] public SystemLanguage SystemLanguage { get; set; }

        public Language(SystemLanguage language) => SystemLanguage = language;

        #region Methods for comparing languages
        public override bool Equals(object obj) => (obj is Language language)? SystemLanguage.Equals(language.SystemLanguage) : false;

        public override int GetHashCode() => SystemLanguage.GetHashCode();

        public override string ToString() => SystemLanguage.ToString();
        #endregion
    }
}