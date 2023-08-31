using UnityEngine;
using UnityEngine.UI;
using EasyAssetsLocalize;

public class LocalizationExample : MonoBehaviour
{
    [SerializeField] LocalizationStorage storage;
    [SerializeField] Text language;

    // Start is called before the first frame update
    public void OnEnable()
    {
        LocalizationManager.Storage = storage;
        SetLanguge(LocalizationManager.Language);
    }

    public void OnValidate()
    {
        Debug.Log("OnValidate");
    }

    public void PrevLanguage()
    {
        LocalizationManager.SetPrevLanguage();
        SetLanguge(LocalizationManager.Language);
    }

    public void NextLanguage()
    { 
        LocalizationManager.SetNextLanguage();
        SetLanguge(LocalizationManager.Language);
    }

    private void SetLanguge(Language language)
    {
        this.language.text = language.ToString();
    }

    public void OnDisable()
    {
        
    }
}
