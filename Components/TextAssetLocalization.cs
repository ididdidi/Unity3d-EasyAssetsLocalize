// Class generated automatically
using UnityEngine.Events;

namespace SimpleLocalization
{
    public class TextAssetLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.TextAsset> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.TextAsset);

        public override void SetData(object data) => handler?.Invoke((UnityEngine.TextAsset)data);
    }
}