namespace ResourceLocalization
{
    /// <summary>
    /// Interface for interacting with resources.
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Property to get and set data
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// Method for making a copy of an object.
        /// </summary>
        /// <returns>Object with similar data</returns>
        IResource Clone();
    }
}
