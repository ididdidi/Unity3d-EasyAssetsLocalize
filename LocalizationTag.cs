using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class LocalizationTag
    {
        [SerializeField] private string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private string name;

        public string ID { get => id; }
        public string Name { get => name; set => name = value; }

        public LocalizationTag(string name)
        {
            this.name = name;
        }

        public override bool Equals(object @object)
        {
            if(@object is LocalizationTag tag)
            {
                return id.Equals(tag.id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}