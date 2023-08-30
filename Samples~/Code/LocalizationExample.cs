using UnityEngine;
using UnityEngine.UI;
using EasyAssetsLocalize;

public class LocalizationExample : MonoBehaviour
{
    [SerializeField] Text language;
    // Start is called before the first frame update
    void Start()
    {
        SetLanguge(LocalizationManager.Language);
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
}
