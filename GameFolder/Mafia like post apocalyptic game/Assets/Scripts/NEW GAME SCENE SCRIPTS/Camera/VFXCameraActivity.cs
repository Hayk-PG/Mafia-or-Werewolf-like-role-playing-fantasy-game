using UnityEngine;

public class VFXCameraActivity : CameraBaseActivity
{
    void Update()
    {
        if (_Options._OptionsUI.OptionsTab.interactable || _InformPlayerRole._UI.CanvasGroup.interactable || _ChatSizeController.IsChatMaximized || _CardsTabController.CardsTabCanvasGroup.interactable || _EndTab._UI.CanvasGroup.interactable)
            _Camera.enabled = false;
        else if(!_Options._OptionsUI.OptionsTab.interactable && !_InformPlayerRole._UI.CanvasGroup.interactable && !_ChatSizeController.IsChatMaximized && !_CardsTabController.CardsTabCanvasGroup.interactable && !_EndTab._UI.CanvasGroup.interactable)
            _Camera.enabled = true;
    }  
}
