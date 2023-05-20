using UnityEngine;

namespace ResourceLocalization
{
    public abstract class ReceiversLocalizationTag<T> : LocalizationTag where T : Object
    {
        [SerializeField, HideInInspector] public T[] receivers = new T[0];

        public override Resource Resource
        {
            get
            {
                var resource = GetResource(null);
                for (int i = 0; i < receivers.Length; i++)
                {
                    if (receivers[i]) { resource = GetResource(receivers[i]); break; }
                }
                return resource;
            }

            set
            {
                for (int i = 0; i < receivers.Length; i++)
                {
                    if (receivers[i]) { SetResource(receivers[i], value); }
                }
            }

        }

        protected abstract Resource GetResource(T reciver);

        protected abstract void SetResource(T reciver, Resource resource);
    }
}
