// Class generated automatically
using UnityEngine.Events;

namespace SimpleLocalization
{
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<System.String> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(System.String);

        public override void SetLocalizationData(object data) => handler?.Invoke((System.String)data);
    }
}