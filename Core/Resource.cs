namespace ResourceLocalization
{
    /// <summary>
    /// Base class for storing localization resources.
    /// </summary>
    [System.Serializable]
    public abstract class Resource
    {
        public abstract System.Type Type { get; }
        public abstract object Data { get; set; }

        public abstract Resource Clone();
    }
}