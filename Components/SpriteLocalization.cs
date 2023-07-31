// Class generated automatically
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class SpriteLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.Sprite> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.Sprite);

        public override void SetLocalizationData(object data) => handler?.Invoke((UnityEngine.Sprite)data);
    }
}