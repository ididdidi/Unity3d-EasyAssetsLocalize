// Class generated automatically
using UnityEngine.Events;

namespace EasyLocalization
{
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<System.String> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(System.String);

        public override void SetData(object data) => handler?.Invoke((System.String)data);
    }
}