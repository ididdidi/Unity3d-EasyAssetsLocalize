namespace EasyAssetsLocalize
{
    /// <summary>
    /// Interface for interacting with resources.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Property to get and set data.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// Method for creating new copies of a resource instance.
        /// </summary>
        /// <returns>Object with similar data</returns>
        IResource Clone();
    }
}
