// Class generated automatically
using UnityEngine.Events;

namespace EasyAssetsLocalize
{
    [UnityEngine.AddComponentMenu("Localize/String Localization")]
    public class StringLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<System.String> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(System.String);

        public override void SetData(object data) => handler?.Invoke((System.String)data);
    }
}