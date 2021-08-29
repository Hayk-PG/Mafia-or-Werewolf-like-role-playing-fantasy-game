using UnityEngine;
using UnityEngine.UI;

public class JoinRoomErrorTab : MonoBehaviour
{
    [SerializeField] Text errorText;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Button closeButton;


    void Update()
    {
        OnClickCloseButton();
    }

    public void OnError(string errorMessage)
    {
        errorText.text = errorMessage;
        MyCanvasGroups.CanvasGroupActivity(canvasGroup, true);
        MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.NetworkManagerComponents.NetworkUI.CreateRoomButtonTab, false);
    }

    void OnClickCloseButton()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => 
        {
            MyCanvasGroups.CanvasGroupActivity(canvasGroup, false);
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.NetworkManagerComponents.NetworkUI.CreateRoomButtonTab, true);
        });
    }
}
