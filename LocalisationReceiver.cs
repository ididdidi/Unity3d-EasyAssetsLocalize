namespace ResourceLocalization
{
    public interface LocalizationReceiver
    {
        string Name { get; set; }

        void SetLocalization(object data);
    }
}
