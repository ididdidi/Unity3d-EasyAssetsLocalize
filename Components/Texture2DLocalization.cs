// Class generated automatically
using UnityEngine;
using UnityEngine.Events;

namespace ResourceLocalization
{
    public class Texture2DLocalization : LocalizationComponent
    {
        [System.Serializable] public class Handler : UnityEvent<Texture2D> { }
        [SerializeField, HideInInspector] private Handler handler;

        public override System.Type Type => typeof(Texture2D);

        public override void SetLocalizationData(object data) => handler?.Invoke((Texture2D)data);
    }
}