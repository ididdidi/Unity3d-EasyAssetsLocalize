// Class generated automatically
using UnityEngine.Events;

namespace SimpleLocalization
{
    public class Texture2DLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.Texture2D> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.Texture2D);

        public override void SetLocalizationData(object data) => handler?.Invoke((UnityEngine.Texture2D)data);
    }
}