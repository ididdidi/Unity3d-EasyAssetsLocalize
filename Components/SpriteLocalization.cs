// Class generated automatically
using UnityEngine.Events;

namespace EasyAssetsLocalize
{
    [UnityEngine.AddComponentMenu("Localize/Sprite Localization")]
    public class SpriteLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<UnityEngine.Sprite> { }
        [UnityEngine.SerializeField, UnityEngine.HideInInspector] private Handler handler;

        public override System.Type Type => typeof(UnityEngine.Sprite);

        protected override void SetData(object data) => handler?.Invoke((UnityEngine.Sprite)data);
    }
}