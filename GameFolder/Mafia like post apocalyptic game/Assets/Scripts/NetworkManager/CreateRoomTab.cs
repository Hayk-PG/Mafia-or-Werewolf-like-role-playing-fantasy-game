using UnityEngine;
using UnityEngine.UI;

public class CreateRoomTab : MonoBehaviour
{
    [SerializeField] InputField roomNameInputField;
    [SerializeField] CanvasGroup nameAndPasswordTabCanvasGroup;
    [SerializeField] CanvasGroup playerCountTabCanvasGroup;
    [SerializeField] CanvasGroup errorPanelCanvasGroup;
    [SerializeField] Text errorText;
    [SerializeField] Button continueButton;
    [SerializeField] Button nameAndPasswordTabBackButton;
    [SerializeField] Button playerCountTabBackButton;
    [SerializeField] Button closeErrorTabButton;
    

    void Start()
    {
        SubToEvents.SubscribeToEvents(PlayerBaseConditions.IsNetworkManagerComponentsNotNull, () => 
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkManager.OnCreateRoomError += NetworkManager_OnCreateRoomError;
        });
    }

    void Update()
    {
        OnClickButtons(continueButton);
        OnClickButtons(nameAndPasswordTabBackButton);
        OnClickButtons(playerCountTabBackButton);
        OnClickButtons(closeErrorTabButton);
    }

    void OnDisable()
    {
        if (PlayerBaseConditions.IsNetworkManagerComponentsNotNull)
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkManager.OnCreateRoomError -= NetworkManager_OnCreateRoomError;
        }
    }

    #region OnClickButtons
    void OnClickButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == continueButton)
            {
                MyCanvasGroups.CanvasGroupActivity(nameAndPasswordTabCanvasGroup, false);
                MyCanvasGroups.CanvasGroupActivity(playerCountTabCanvasGroup, true);
            }
            if(button == nameAndPasswordTabBackButton)
            {
                PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnBackFromCreateRoomTab();
            }
            if (button == playerCountTabBackButton)
            {
                MyCanvasGroups.CanvasGroupActivity(playerCountTabCanvasGroup, false);
                MyCanvasGroups.CanvasGroupActivity(nameAndPasswordTabCanvasGroup, true);
            }
            if (button == closeErrorTabButton)
            {
                MyCanvasGroups.CanvasGroupActivity(errorPanelCanvasGroup, false);
                MyCanvasGroups.CanvasGroupActivity(nameAndPasswordTabCanvasGroup, true);
                errorText.text = null;
                roomNameInputField.text = null;
            }
        });
    }
    #endregion

    #region NetworkManager_OnCreateRoomError
    void NetworkManager_OnCreateRoomError(string obj)
    {
        errorText.text = obj;
        MyCanvasGroups.CanvasGroupActivity(errorPanelCanvasGroup, true);
        MyCanvasGroups.CanvasGroupActivity(nameAndPasswordTabCanvasGroup, false);
        MyCanvasGroups.CanvasGroupActivity(playerCountTabCanvasGroup, false);
        PlayerBaseConditions.UiSounds.PlaySoundFX(7);
    }
    #endregion  
}
