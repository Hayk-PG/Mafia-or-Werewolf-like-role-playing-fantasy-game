using UnityEngine;
using UnityEngine.UI;

public class FriendButtonScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text friendNameText;
    [SerializeField] Button friendProfileButton;
    [SerializeField] Button deleteFriendButton;
    [SerializeField] Image statusImage;
    
    public string Name
    {
        get => transform.name;
        set => transform.name = value;
    }
    public string FriendName
    {
        get => friendNameText.text;
        set => friendNameText.text = value;
    }  
    public Color32 StatusImageColor
    {
        get => statusImage.color;
        set => statusImage.color = value;
    }


    void Update()
    {
        friendProfileButton.onClick.RemoveAllListeners();
        friendProfileButton.onClick.AddListener(() => 
        {
            OnClickFriendButton(GetComponent<FriendButtonScript>());
        });

        OnClickDeleteFriendButton();
    }

    void OnClickFriendButton(FriendButtonScript friend)
    {
        PlayerBaseConditions.PlayerProfile.FriendProfileFriendName = friend.FriendName;
        PlayerBaseConditions.PlayerProfile.ShowPlayerProfilePic(friend.Name);
        PlayerBaseConditions.PlayerProfile.FriendProfileFriendMessageButton.name = Name;
        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(friend.Name,
            GetFriendStats => 
            {
                PlayerBaseConditions.PlayerProfile.FriendProfileRankNumber = GetFriendStats.rank.ToString();
            });

        MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[6], true);
    }

    void OnClickDeleteFriendButton()
    {
        deleteFriendButton.onClick.RemoveAllListeners();
        deleteFriendButton.onClick.AddListener(() =>
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.UnFriend(PlayerBaseConditions.OwnPlayfabId, transform.name);
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.UnFriend(transform.name, PlayerBaseConditions.OwnPlayfabId);
        });
    }
}
