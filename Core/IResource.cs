namespace ResourceLocalization
{
    public interface IResource
    {
        object Data { get; set; }
        IResource Clone();
    }
}
