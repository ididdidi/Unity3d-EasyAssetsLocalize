// Class generated automatically
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class TextureLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<Texture> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(Texture);

        public override void SetLocalizationData(object data) => handler?.Invoke((Texture)data);
    }
}