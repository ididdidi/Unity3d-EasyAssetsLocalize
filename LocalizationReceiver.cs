using UnityEngine;

namespace ResourceLocalization
{
    public abstract class LocalizationReceiver : MonoBehaviour
    {
        [SerializeField, HideInInspector] private string id;
        public string ID { get => id; set => id = value; }
        public abstract Resource Resource { get; set; }
    }
}
