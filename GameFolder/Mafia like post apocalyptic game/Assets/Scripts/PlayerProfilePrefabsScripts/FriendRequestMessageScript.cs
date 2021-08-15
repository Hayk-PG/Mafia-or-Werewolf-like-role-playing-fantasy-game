using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendRequestMessageScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button acceptButton;
    [SerializeField] Button denyButton;
    [SerializeField] Text messageText;

    [Header("ADDRESSEE")]
    [SerializeField] string addressee;

    public string Name
    {
        get => transform.name;
        set => transform.name = value;
    }
    public string Message
    {
        get => messageText.text;
        set => messageText.text = value;
    }
    public string Addressee
    {
        get => addressee;
        set => addressee = value;
    }


    void Update()
    {
        OnClickButtons(acceptButton);
        OnClickButtons(denyButton);
    }

    #region OnClickButtons
    void OnClickButtons(Button button)
    {
        if(button == acceptButton)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => 
            {
                PlayerBaseConditions.PlayfabManager.PlayfabFriends.SearchPlayerInFriendsList(PlayerBaseConditions.OwnPlayfabId, Addressee, 
                    friend => 
                    {
                        if(friend == null)
                        {
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.AddFriend(PlayerBaseConditions.OwnPlayfabId, Addressee);
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.AddFriend(Addressee, PlayerBaseConditions.OwnPlayfabId);
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(PlayerBaseConditions.OwnPlayfabId, PlayerKeys.FriendRequest.FR + Addressee);
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(Addressee, PlayerKeys.FriendRequest.FR + PlayerBaseConditions.OwnPlayfabId);
                            Destroy(gameObject);
                        }
                        else
                        {
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(PlayerBaseConditions.OwnPlayfabId, PlayerKeys.FriendRequest.FR + Addressee);
                            PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(Addressee, PlayerKeys.FriendRequest.FR + PlayerBaseConditions.OwnPlayfabId);
                            Destroy(gameObject);
                        }
                    });                
            });            
        }
        if(button = denyButton)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(PlayerBaseConditions.OwnPlayfabId, PlayerKeys.FriendRequest.FR + Addressee);
                PlayerBaseConditions.PlayfabManager.PlayfabFriends.DeleteData(Addressee, PlayerKeys.FriendRequest.FR + PlayerBaseConditions.OwnPlayfabId);
                Destroy(gameObject);
            });            
        }
    }
    #endregion

}
