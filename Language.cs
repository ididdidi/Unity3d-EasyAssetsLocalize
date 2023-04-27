using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class Language
    {
        [SerializeField] private string name;

        public Language(string name)
        {
            this.name = name;
        }

        public string Name { get => name; set => name = value; }

        public override bool Equals(object obj) => (obj is Language language)? name.Equals(language.name) : false;

        public override int GetHashCode() => name.GetHashCode();

        public override string ToString() => name;
    }
}