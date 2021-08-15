using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUIButtons : MonoBehaviour
{
    #region EVENTS
    public event Action OnClickBadgeButton;
    public event Action<bool> OnClickSignUpInButton;
    public event Action<string, string> OnPlayerNickName;
    public event Action OnClickCreateButton;
    public event Action OnClickFindButton;
    public event Action<IRoomButton> OnClickRoomButton;
    #endregion

    [Header("BUTTONS")]   
    [SerializeField] Button badgeButton;
    [SerializeField] Button[] singUpInButtons;
    [SerializeField] Button createButton;
    [SerializeField] Button findButton;
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
    public Button BadgeButton => badgeButton;



    void Update()
    {
        OnBadgeButton();
        OnSignUpInUpButton();
        OnCreateButton();
        OnFindButton();
        OnRoomButtons();
    }

    void OnBadgeButton()
    {
        badgeButton.onClick.RemoveAllListeners();
        badgeButton.onClick.AddListener(() => { OnClickBadgeButton?.Invoke(); });
    }

    void OnSignUpInUpButton()
    {
        for (int i = 0; i < singUpInButtons.Length; i++)
        {
            int index = i;

            singUpInButtons[index].onClick.RemoveAllListeners();
            singUpInButtons[index].onClick.AddListener(() => { OnClickSignUpInButton?.Invoke(index == 0 ? true:false); });
        }
    }

    void OnCreateButton()
    {
        createButton.onClick.RemoveAllListeners();
        createButton.onClick.AddListener(() => { OnClickCreateButton?.Invoke(); });
    }

    void OnFindButton()
    {
        findButton.onClick.RemoveAllListeners();
        findButton.onClick.AddListener(() => { OnClickFindButton?.Invoke(); });
    }

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








}
