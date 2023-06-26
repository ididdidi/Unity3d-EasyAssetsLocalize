using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationComponent : MonoBehaviour
    {
        [field: SerializeField, HideInInspector] public string ID { get; set; }

        public LocalizationStorage Storage { get => LocalizationManager.LocalizationStorage; }

        public abstract System.Type Type { get; }

        public abstract void SetLocalizationData(object data);

        private void Awake()
        {
            if (!string.IsNullOrEmpty(ID)) { LocalizationManager.AddLocalizationComponent(this); }
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(ID)) { LocalizationManager.RemoveLocalizationComponent(this); }
        }
    }
}
