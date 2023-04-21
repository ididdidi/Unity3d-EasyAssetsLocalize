using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationReceiver : MonoBehaviour
    {
        [SerializeField] private Tag localizationTag;
        public Tag LocalizationTag { get => localizationTag; set => localizationTag = value; }
        public abstract Resource Resource { get; set; }
    }
}
