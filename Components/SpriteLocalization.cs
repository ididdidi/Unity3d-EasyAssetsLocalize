using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class SpriteLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<Sprite> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(Sprite);

        public override void SetLocalizationData(object data) => handler?.Invoke((Sprite) data);
    }
}
