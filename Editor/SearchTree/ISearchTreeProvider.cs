namespace EasyAssetsLocalize
{
    /// <summary>
    /// An interface that defines the methods needed to provide data for searching 
    /// and respond to the selection of entries in the search results.
    /// </summary>
    public interface ISearchTreeProvider
    {
        /// <summary>
        /// Method for obtaining initial data for search
        /// </summary>
        /// <returns><see cref="SearchTree"/></returns>
        SearchTree GetSearchTree();
    }
}