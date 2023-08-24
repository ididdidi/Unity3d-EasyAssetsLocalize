using UnityEngine;

namespace EasyAssetsLocalize
{    /// <summary>
     /// Encapsulates object resource data.
     /// </summary>
    [System.Serializable]
    public class UnityResource : IResource
    {
        [SerializeField] private Object data;

        /// <summary>
        /// Property to get and set data.
        /// </summary>
        public object Data { get => data; set => data = (Object)value; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">Resource data</param>
        public UnityResource(object data) => Data = data ?? throw new System.ArgumentNullException(nameof(data));

        /// <summary>
        /// Method for creating new copies of a resource instance.
        /// </summary>
        /// <returns><see cref="IResource"/></returns>
        public IResource Clone() => new UnityResource(data);
    }
}
