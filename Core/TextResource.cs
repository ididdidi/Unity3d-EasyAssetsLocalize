using UnityEngine;

namespace EasyAssetsLocalize
{
    /// <summary>
    /// Encapsulates text resource data.
    /// </summary>
    [System.Serializable]
    public class TextResource : IResource
    {
        [SerializeField] private string data;

        /// <summary>
        /// Property to get and set data.
        /// </summary>
        public object Data { get => data; set => data = (string)value; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="data">Resource data</param>
        public TextResource(object data) => Data = data ?? throw new System.ArgumentNullException(nameof(data));

        /// <summary>
        /// Method for creating new copies of a resource instance.
        /// </summary>
        /// <returns><see cref="IResource"/></returns>
        public IResource Clone() => new TextResource(data);
    }
}
