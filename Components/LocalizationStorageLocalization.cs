// Class generated automaticallyusing ResourceLocalization;
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class LocalizationStorageLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<LocalizationStorage> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(LocalizationStorage);

        public override void SetLocalizationData(object data) => handler?.Invoke((LocalizationStorage)data);
    }
}