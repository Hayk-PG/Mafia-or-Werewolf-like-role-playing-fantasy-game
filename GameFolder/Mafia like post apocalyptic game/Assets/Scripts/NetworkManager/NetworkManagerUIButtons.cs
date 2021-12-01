using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUIButtons : MonoBehaviour
{
    #region EVENTS
    public event Action OnClickBadgeButton;
    public event Action<bool> OnClickSignUpInButton;
    public event Action OnClickCreateRoomButton;
    public event Action<IRoomButton> OnClickRoomButton;
    #endregion

    [Header("BUTTONS")]   
    [SerializeField] Button badgeButton;
    [SerializeField] Button[] singUpInButtons;
    [SerializeField] Button createRoomButton;

    /// <summary>
    /// All room buttons in Lobby tab
    /// </summary>
    public Button[] RoomButtons
    {
        get
        {
            if(NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.GetComponentsInChildren<Button>() != null)
            {
                return NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.GetComponentsInChildren<Button>();
            }
            else
            {
                return null;
            }
        }
    }
    public Button BadgeButton
    {
        get => badgeButton;
        set => badgeButton = value;
    }


    void Start()
    {
        BadgeButton = FindObjectOfType<PlayerBadgeButton>().GetComponent<Button>();
    }

    void Update()
    {
        OnBadgeButton();
        OnSignUpInUpButton();
        OnCreateButton();
        OnRoomButtons();
    }

    #region OnSignUpInUpButton
    void OnSignUpInUpButton()
    {
        for (int i = 0; i < singUpInButtons.Length; i++)
        {
            int index = i;

            singUpInButtons[index].onClick.RemoveAllListeners();
            singUpInButtons[index].onClick.AddListener(() => 
            {
                OnClickSignUpInButton?.Invoke(index == 0 ? true : false);
            });
        }
    }
    #endregion

    #region OnCreateButton
    void OnCreateButton()
    {
        createRoomButton.onClick.RemoveAllListeners();
        createRoomButton.onClick.AddListener(() => { OnClickCreateRoomButton?.Invoke();});
    }
    #endregion

    #region OnBadgeButton
    void OnBadgeButton()
    {
        badgeButton.onClick.RemoveAllListeners();
        badgeButton.onClick.AddListener(() => { OnClickBadgeButton?.Invoke(); });
    }
    #endregion

    #region OnRoomButtons
    void OnRoomButtons()
    {
        if(RoomButtons != null)
        {
            foreach (var room in RoomButtons)
            {
                room.onClick.RemoveAllListeners();
                room.onClick.AddListener(() => { OnClickRoomButton?.Invoke(room.GetComponent<IRoomButton>()); });
            }
        }
    }
    #endregion







}
