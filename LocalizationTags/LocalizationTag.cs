using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationTag : MonoBehaviour
    {
        [SerializeField, HideInInspector] private string id;
        public string ID { get => id; set => id = value; }

        public Resource Resource { get => GetResource(); set => SetResource(value); }

        protected abstract Resource GetResource();

        protected abstract void SetResource(Resource resource);
    }
}
