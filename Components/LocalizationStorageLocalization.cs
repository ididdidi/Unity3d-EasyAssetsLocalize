// Class generated automatically
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class LocalizationStorageLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<ResourceLocalization.LocalizationStorage> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(ResourceLocalization.LocalizationStorage);

        public override void SetLocalizationData(object data) => handler?.Invoke((ResourceLocalization.LocalizationStorage)data);
    }
}