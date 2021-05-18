using UnityEngine;

[System.Serializable]
public class LocalizationResource
{
    [SerializeField] private string tag;
    [SerializeField] private string stringData;

    public string Tag { get => tag; }

    public string StringData { get => stringData; }

    public LocalizationResource(string tag, string value)
    {
        this.tag = tag;
        stringData = value;
    }
}
