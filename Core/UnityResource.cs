using UnityEngine;

namespace ResourceLocalization
{
    [System.Serializable]
    public class UnityResource : IResource
    {
        [SerializeField] private Object data;

        public object Data { get => data; set => data = (Object)value; }

        public UnityResource(object data) => Data = data ?? throw new System.ArgumentNullException(nameof(data));

        public IResource Clone() => new UnityResource(data);
    }
}
