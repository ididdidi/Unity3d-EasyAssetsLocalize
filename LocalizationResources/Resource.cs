namespace ResourceLocalization
{
    [System.Serializable]
    public abstract class Resource
    {
        public abstract System.Type Type { get; }
        public abstract object Data { get; set; }

        public abstract Resource Clone();
    }
}