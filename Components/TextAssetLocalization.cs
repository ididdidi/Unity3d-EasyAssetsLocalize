// Class generated automatically
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class TextAssetLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.TextAsset> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.TextAsset);

        public override void SetLocalizationData(object data) => handler?.Invoke((UnityEngine.TextAsset)data);
    }
}