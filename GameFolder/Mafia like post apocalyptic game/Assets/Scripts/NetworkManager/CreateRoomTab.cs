using UnityEngine;
using UnityEngine.UI;

public class CreateRoomTab : MonoBehaviour
{
    [SerializeField] InputField roomNameInputField;
    [SerializeField] CanvasGroup errorPanelCanvasGroup;
    [SerializeField] Text errorText;
    [SerializeField] Button backButton;

    void Start()
    {
        SubToEvents.SubscribeToEvents(PlayerBaseConditions.IsNetworkManagerComponentsNotNull, () => 
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkManager.OnCreateRoomError += NetworkManager_OnCreateRoomError;
        });
    }

    void Update()
    {
        OnClickInputField();
        OnClickBackButton();
    }

    void OnDisable()
    {
        if (PlayerBaseConditions.IsNetworkManagerComponentsNotNull)
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkManager.OnCreateRoomError -= NetworkManager_OnCreateRoomError;
        }
    }

    #region NetworkManager_OnCreateRoomError
    void NetworkManager_OnCreateRoomError(string obj)
    {
        errorText.text = obj;
        MyCanvasGroups.CanvasGroupActivity(errorPanelCanvasGroup, true);
    }
    #endregion

    #region OnClickInputField
    void OnClickInputField()
    {
        if (roomNameInputField.isFocused)
        {
            if (errorPanelCanvasGroup.interactable)
            {               
                MyCanvasGroups.CanvasGroupActivity(errorPanelCanvasGroup, false);
                errorText.text = null;
                roomNameInputField.text = null;
            }
        }
    }
    #endregion

    #region OnClickBackButton
    void OnClickBackButton()
    {
        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(() => 
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnBackFromCreateRoomTab();
        });
    }
    #endregion
}
