using System;
using UnityEngine;
using UnityEngine.UI;

public class FriendButtonScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text friendNameText;
    [SerializeField] Button friendProfileButton;
    [SerializeField] Button deleteFriendButton;
    [SerializeField] Image statusImage;

    [Serializable] public class Room
    {
        [SerializeField] bool isInRoom;
        [SerializeField] string roomName;

        public bool IsInRoom
        {
            get => isInRoom;
            set => isInRoom = value;
        }
        public string RoomName
        {
            get => roomName;
            set => roomName = value;
        }
    }
    public Room _Room;
    
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
            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
        });

        OnClickDeleteFriendButton();
    }

    void OnClickFriendButton(FriendButtonScript friend)
    {
        PlayerBaseConditions.PlayerProfile.FriendProfileFriendName = friend.FriendName;
        PlayerBaseConditions.PlayerProfile.ShowPlayerProfilePic(friend.Name);
        PlayerBaseConditions.PlayerProfile.FriendProfileFriendMessageButton.name = Name;

        PlayerBaseConditions.PlayerProfile.JoinFriendsRoomButton.gameObject.SetActive(_Room.IsInRoom);
        PlayerBaseConditions.PlayerProfile.JoinFriendsRoomButton.gameObject.name = _Room.RoomName;

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
            PlayerBaseConditions.UiSounds.PlaySoundFX(5);
            Destroy(gameObject);
        });
    }
}
