using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for storing language information.
    /// </summary>
    [System.Serializable]
    public class Language
    {
        public Language(string name) => Name = name;

        /// <summary>
        /// Language name.
        /// </summary>
        [field: SerializeField] public string Name { get; set; }

        #region Methods for comparing languages
        public override bool Equals(object obj) => (obj is Language language)? Name.Equals(language.Name) : false;

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
        #endregion
    }
}