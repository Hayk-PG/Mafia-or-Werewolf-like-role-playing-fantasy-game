using UnityEngine;

public class VFXCameraActivity : MonoBehaviour
{
    Camera _Camera { get; set; }

    Options _Options { get; set; }
    InformPlayerRole _InformPlayerRole { get; set; }
    ChatSizeController _ChatSizeController { get; set; }
    CardsTabController _CardsTabController { get; set; }

    void Awake()
    {
        _Camera = GetComponent<Camera>();
        _CardsTabController = FindObjectOfType<CardsTabController>();
        _InformPlayerRole = FindObjectOfType<InformPlayerRole>();
        _ChatSizeController = FindObjectOfType<ChatSizeController>();
        _Options = FindObjectOfType<Options>();
    }

    void Update()
    {
        if (_Options._OptionsUI.OptionsTab.interactable || _InformPlayerRole._UI.CanvasGroup.interactable || _ChatSizeController.IsChatMaximized || _CardsTabController.CardsTabCanvasGroup.interactable)
            _Camera.enabled = false;
        else if(!_Options._OptionsUI.OptionsTab.interactable && !_InformPlayerRole._UI.CanvasGroup.interactable && !_ChatSizeController.IsChatMaximized && !_CardsTabController.CardsTabCanvasGroup.interactable)
            _Camera.enabled = true;
    } 

    
}
