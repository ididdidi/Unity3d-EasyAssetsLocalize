using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationComponent : MonoBehaviour
    {
        [field: SerializeField] public LocalizationTag Tag { get; set; }

        public LocalizationStorage Storage { get => LocalizationManager.LocalizationStorage; }

        public abstract System.Type Type { get; }

        public abstract void SetLocalization(object data);

        private void Awake()
        {
            LocalizationManager.AddLocalizationComponent(this);
        }

        private void OnDestroy()
        {
            LocalizationManager.RemoveLocalizationComponent(this);
        }
    }
}
