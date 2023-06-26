using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<string> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(string);

        public override void SetLocalizationData(object data) => handler?.Invoke((string)data);
    }
}
