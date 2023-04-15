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

            void SetData(object data);
        }

        [SerializeField] private string id = System.Guid.NewGuid().ToString().Replace("-", "");
        [SerializeField] private Object receiver;

        public string ID { get => id; }
        public IReceiver Object => receiver as IReceiver;

    }
}
