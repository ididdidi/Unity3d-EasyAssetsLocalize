using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class LocalizationReceiver
    {
        public interface IReceiver
        {
            string Name { get; }
            System.Type ResourceType { get; }

            object Data { get; set; }
        }

        [SerializeField] private string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private Object receiver;

        public string ID { get => id; }
        public IReceiver Object { 
            get => (IReceiver)receiver; 
            set => receiver = (Object)value;
        }

    }
}
