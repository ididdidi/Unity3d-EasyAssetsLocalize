namespace EasyLocalization
{
    /// <summary>
    /// An interface that defines the methods needed to provide data for searching 
    /// and respond to the selection of entries in the search results.
    /// </summary>
    public interface ISearchTreeProvider
    {
        SearchTree GetSearchTree();
    }
}