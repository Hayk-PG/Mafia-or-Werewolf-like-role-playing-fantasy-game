using UnityEngine;
using UnityEngine.UI;

public class SendFriendRequestButton : MonoBehaviour
{
    Button sendFriendRequestButton;

    void Start()
    {
        sendFriendRequestButton = PlayerBaseConditions.PlayerProfile.SendFriendRequestButton;
    }

    void Update()
    {
        OnClickSendFriendRequestButton();
    }

    void OnClickSendFriendRequestButton()
    {
        sendFriendRequestButton.onClick.RemoveAllListeners();
        sendFriendRequestButton.onClick.AddListener(() =>
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.SendFriendRequest(sendFriendRequestButton.name);
        });
    }



}
