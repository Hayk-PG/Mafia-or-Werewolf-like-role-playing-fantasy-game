using UnityEngine;
using UnityEngine.UI;

public class RoomButtonScript : MonoBehaviour, IRoomButton
{
    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup roomCanvasGroup;
    [SerializeField] CanvasGroup passwordCanvasGroup;

    [Header("PIN")]
    [SerializeField] string pin;

    [Header("UI")]
    [SerializeField] Text roomNameText;
    [SerializeField] Text roomPlayersCountText;
    [SerializeField] Image openImage;
    [SerializeField] Image lockImage;
    [SerializeField] Button confirmPinButton;
    [SerializeField] InputField passwordInputField;

    [Header("SPRITES")]
    [SerializeField] Sprite lockedSprite;
    [SerializeField] Sprite unlockedSprite;
    [SerializeField] Sprite openSprite;
    [SerializeField] Sprite closeSprite;

    #region IRoomButton
    public string Pin
    {
        get
        {
            return pin;
        }
        set
        {
            pin = value;
        }
    }
    public string RoomButtonName
    {
        get
        {
            return transform.name;
        }
        set
        {
            transform.name = value;
        }
    }
    public string RoomName
    {
        get
        {
            return roomNameText.text;
        }
        set
        {
            roomNameText.text = value;
        }
    }
    public string RoomPlayersCount
    {
        get
        {
            return roomPlayersCountText.text;
        }
        set
        {
            roomPlayersCountText.text = value;
        }
    }
    public Sprite OpenSprite
    {
        get
        {
            return openImage.sprite;
        }
        set
        {
            openImage.sprite = value;
        }
    }
    public Sprite LockSprite
    {
        get
        {
            return lockImage.sprite;
        }
        set
        {
            lockImage.sprite = value;
        }
    }
    #endregion


    void Update()
    {
        OnClickConfirmPinButton();
    }

    #region CanvasGroupActivity
    void CanvasGroupActivity(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
    #endregion
 
    #region UpdateRoomButton
    public void UpdateRoomButton(string roomName, int playersCount, int maxPlayers, bool isOpen, bool isLocked, string pin)
    {
        RoomButtonName = roomName;
        RoomName = roomName;
        RoomPlayersCount = playersCount + "/" + maxPlayers;
        OpenSprite = isOpen ? openSprite : closeSprite;
        LockSprite = isLocked ? lockedSprite : unlockedSprite;
        Pin = pin;
    }
    
    public void UpdateRoomButton(int playersCount, int maxPlayers, bool isOpen)
    {
        RoomPlayersCount = playersCount + "/" + maxPlayers;
        OpenSprite = isOpen ? openSprite : closeSprite;
    }
    #endregion

    #region OnClickConfirmPinButton
    public void OnClickConfirmPinButton()
    {
        confirmPinButton.onClick.RemoveAllListeners();
        confirmPinButton.onClick.AddListener(delegate
        {
            if(passwordInputField.text == Pin)
            {
                Photon.Pun.PhotonNetwork.JoinRoom(RoomName);
            }
            else
            {
                EnableRoomCanvasGroup();
                passwordInputField.text = null;
            }
        });
    }
    #endregion

    #region EnableRoomCanvasGroup
    public void EnableRoomCanvasGroup()
    {
        CanvasGroupActivity(roomCanvasGroup, true);
        CanvasGroupActivity(passwordCanvasGroup, false);
    }
    #endregion

    #region EnablePasswordCanvasGroup
    public void EnablePasswordCanvasGroup()
    {
        CanvasGroupActivity(roomCanvasGroup, false);
        CanvasGroupActivity(passwordCanvasGroup, true);
    }
    #endregion
}
