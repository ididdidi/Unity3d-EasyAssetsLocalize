using UnityEngine;

namespace ResourceLocalization
{
    /// <summary>
    /// Class for storing language information.
    /// </summary>
    [System.Serializable]
    public class Language
    {
        [SerializeField] private string name;

        public Language(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Language name.
        /// </summary>
        public string Name { get => name; set => name = value; }

        #region Methods for comparing languages
        public override bool Equals(object obj) => (obj is Language language)? name.Equals(language.name) : false;

        public override int GetHashCode() => name.GetHashCode();

        public override string ToString() => name;
        #endregion
    }
}