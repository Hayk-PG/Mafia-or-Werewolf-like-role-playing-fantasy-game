using UnityEngine;
using UnityEngine.UI;

public class UISounds : MonoBehaviour
{
    NetworkManagerComponents NetworkManagerComponents
    {
        get
        {
            if(FindObjectOfType<NetworkManagerComponents>() != null)
            {
                return FindObjectOfType<NetworkManagerComponents>();
            }
            else
            {
                return null;
            }
        }
    }

    protected InputField[] AllInputFields { get; set; }
    protected Toggle[] AllToggles { get; set; }
    protected Button[] AllButtons { get; set; }
    protected Slider[] AllSliders { get; set; }
 

    [Header("AUDIO SOURCES")]
    [SerializeField] protected AudioSource uiSRC;
    [SerializeField] protected AudioSource musicSRC;

    [Header("UI SOUND FX")]
    [SerializeField] protected AudioClip uiPanelPopUpSFX;
    [SerializeField] protected AudioClip uiTypingSFX;
    [SerializeField] protected AudioClip uiToggleSFX;
    [SerializeField] protected AudioClip uiClickSFX;
    [SerializeField] protected AudioClip uiSliderSFX;
   

    protected virtual void Awake()
    {
        AllInputFields = FindObjectsOfType<InputField>();
        AllToggles = FindObjectsOfType<Toggle>();
        AllButtons = FindObjectsOfType<Button>();
        AllSliders = FindObjectsOfType<Slider>();
    }

    protected virtual void OnEnable()
    {
              
    }

    protected virtual void OnDisable()
    {
        
    }

    protected virtual void Update()
    {
        OnInputFieldValueChanged();
        OnToggleValueChanged();
        OnButtonClicked();
        OnSliderValueChanged();
    }

    public void PlayUISoundFX(AudioClip soundFX)
    {
        uiSRC.PlayOneShot(soundFX);
    }

    void NetworkManager_OnConnectedToMasterServer(bool isNickNameSet)
    {
        if (!isNickNameSet)
        {
            PlayUISoundFX(uiPanelPopUpSFX);
        }
    }

    void OnInputFieldValueChanged()
    {
        foreach (var inputField in AllInputFields)
        {
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(delegate { PlayUISoundFX(uiTypingSFX); });
        }
    }

    void OnToggleValueChanged()
    {
        foreach (var toggle in AllToggles)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(delegate { PlayUISoundFX(uiToggleSFX); });
        }
    }

    void OnButtonClicked()
    {
        foreach (var button in AllButtons)
        {
            button.onClick.RemoveListener(delegate { PlayUISoundFX(uiClickSFX); });
            button.onClick.AddListener(delegate { PlayUISoundFX(uiClickSFX); });
        }
    }

    void OnSliderValueChanged()
    {
        foreach (var slider in AllSliders)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(delegate { PlayUISoundFX(uiSliderSFX); });
        }
    }

}
