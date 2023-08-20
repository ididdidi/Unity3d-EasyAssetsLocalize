// Class generated automatically
using UnityEngine.Events;

namespace SimpleLocalization
{
    public class SpriteLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.Sprite> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.Sprite);

        public override void SetData(object data) => handler?.Invoke((UnityEngine.Sprite)data);
    }
}