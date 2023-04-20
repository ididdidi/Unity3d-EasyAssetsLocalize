using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationReceiver : MonoBehaviour
    {
        [SerializeField] private Tag localizationTag;
        public Tag LocalizationTag => localizationTag == null ? new Tag(name) : localizationTag;
        public abstract Resource Resource { get; set; }
    }
}
