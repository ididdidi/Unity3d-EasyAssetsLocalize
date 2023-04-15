namespace ResourceLocalization
{
    public interface LocalizationReceiver
    {
        string Name { get; }
        string ID { get; set; }
        System.Type ResourceType { get; }

        void SetLocalization(object data);
    }
}
