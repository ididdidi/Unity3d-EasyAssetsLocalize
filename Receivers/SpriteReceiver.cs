using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    [System.Serializable]
    public class SpriteReceiver : LocalizationReceiver
    {
        [System.Serializable] public class Handlers : UnityEvent<Sprite> { }
        [SerializeField] private Handlers handlers;

        public SpriteReceiver(LocalizationTag localizationTag) : base(localizationTag) { }

        public override void SetLocalization(object resource)
        {
            handlers?.Invoke((Sprite)resource);
        }
    }
}