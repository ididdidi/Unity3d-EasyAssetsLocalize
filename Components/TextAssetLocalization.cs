// Class generated automatically
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class TextAssetLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<TextAsset> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(TextAsset);

        public override void SetLocalizationData(object data) => handler?.Invoke((TextAsset)data);
    }
}