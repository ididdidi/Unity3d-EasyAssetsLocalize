// Class generated automatically
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<System.String> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(System.String);

        public override void SetLocalizationData(object data) => handler?.Invoke((System.String)data);
    }
}