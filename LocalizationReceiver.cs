using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationReceiver : MonoBehaviour
    {
        [SerializeField] private LocalizationTag localizationTag;
        public LocalizationTag LocalizationTag { get => localizationTag; set => localizationTag = value; }
        public abstract Resource Resource { get; set; }
    }
}
