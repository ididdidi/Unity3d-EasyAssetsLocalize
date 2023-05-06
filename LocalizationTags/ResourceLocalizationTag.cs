using UnityEngine;

namespace ResourceLocalization
{
    public abstract class ResourceLocalizationTag<T> : LocalizationTag where T : Object
    {
        [SerializeReference, HideInInspector] public T[] recivers = new T[0];

        public override Resource Resource
        {
            get
            {
                Resource resource = null;
                for (int i = 0; i < recivers.Length; i++)
                {
                    if (recivers[i]) { resource = GetResource(recivers[i]); break; }
                }
                return resource;
            }

            set
            {
                for (int i = 0; i < recivers.Length; i++)
                {
                    if (recivers[i]) { SetResource(recivers[i], value); }
                }
            }

        }

        protected abstract Resource GetResource(T reciver);

        protected abstract void SetResource(T reciver, Resource resource);
    }
}
