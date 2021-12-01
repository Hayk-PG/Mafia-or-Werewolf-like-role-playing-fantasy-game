using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup playfab_CG;
    [SerializeField] CanvasGroup signUpInTab_CG;
    [SerializeField] CanvasGroup signUpTab_CG;
    [SerializeField] CanvasGroup signInTab_CG;
    [SerializeField] CanvasGroup createRoomButton_CG;
    [SerializeField] CanvasGroup lobbyTab_CG;
    [SerializeField] CanvasGroup createRoomTab_CG;

    [Header("BACKGROUND")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite normalBackground;
    [SerializeField] Sprite bluredBackground;

    public Image BackgroundImage
    {
        get
        {
            return backgroundImage;
        }
        set
        {
            backgroundImage = value;
        }
    }

    public CanvasGroup PlayfabTab => playfab_CG;
    public CanvasGroup SignUpInTab => signUpInTab_CG;
    public CanvasGroup SignUpTab => signUpTab_CG;
    public CanvasGroup SignInTab => signInTab_CG;
    public CanvasGroup CreateRoomButtonTab => createRoomButton_CG;
    public CanvasGroup LobbyTab_CG => lobbyTab_CG;
    public CanvasGroup CreateRoomTab_CG => createRoomTab_CG;
    
    NetworkManagerUIButtons NetworkManagerUIButtons { get; set; }
    NetworkManager NetworkManager { get; set; }


    void Awake()
    {
        NetworkManagerUIButtons = GetComponent<NetworkManagerComponents>().NetworkUIButtons;
        NetworkManager = GetComponent<NetworkManagerComponents>().NetworkManager;
    }

    void OnEnable()
    {
        NetworkManagerUIButtons.OnClickSignUpInButton += NetworkUIButtons_OnClickSignUpInButton;
        NetworkManagerUIButtons.OnClickCreateRoomButton += NetworkManagerUIButtons_OnClickCreateRoomButton;
        NetworkManager.OnLobbyJoined += NetworkManager_OnLobbyJoined;
        NetworkManager.OnRoomCreated += NetworkManager_OnRoomCreated;          
    }
    
    void OnDisable()
    {
        NetworkManagerUIButtons.OnClickSignUpInButton -= NetworkUIButtons_OnClickSignUpInButton;
        NetworkManagerUIButtons.OnClickCreateRoomButton -= NetworkManagerUIButtons_OnClickCreateRoomButton;
        NetworkManager.OnLobbyJoined -= NetworkManager_OnLobbyJoined;
        NetworkManager.OnRoomCreated -= NetworkManager_OnRoomCreated;
    }

    #region NetworkUIButtons_OnClickSignUpInButton
    void NetworkUIButtons_OnClickSignUpInButton(bool isSignUpButtonPressed)
    {
        MyCanvasGroups.CanvasGroupActivity(SignUpInTab, false);

        if (isSignUpButtonPressed)
        {
            MyCanvasGroups.CanvasGroupActivity(SignUpTab, true);
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.NetworkManagerComponents.SignUpTab.BackButtonCanvasGroup, true);
        }
        else
        {
            MyCanvasGroups.CanvasGroupActivity(SignInTab, true);
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.NetworkManagerComponents.SignInTab.BackButtonCanvasGroup, true);
        }
    }
    #endregion

    #region OnLoggedIn
    public void OnLoggedIn()
    {
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, false);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region OnLoggedOut
    public void OnLoggedOut()
    {
        Options.instance.OnPressedOptionsButtons();
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, true);
        MyCanvasGroups.CanvasGroupActivity(SignUpInTab, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region OnLoginToAnotherAccount
    public void OnLoginToAnotherAccount()
    {
        Options.instance.OnPressedOptionsButtons();
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, true);
        MyCanvasGroups.CanvasGroupActivity(SignUpInTab, false);
        MyCanvasGroups.CanvasGroupActivity(SignInTab, true);
        MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.NetworkManagerComponents.SignInTab.BackButtonCanvasGroup, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region NetworkManager_OnLobbyJoined
    void NetworkManager_OnLobbyJoined()
    {
        Options.instance.OnPressedOptionsButtons();
        MyCanvasGroups.CanvasGroupActivity(LobbyTab_CG, true);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomButtonTab, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region NetworkManagerUIButtons_OnClickCreateRoomButton
    void NetworkManagerUIButtons_OnClickCreateRoomButton()
    {
        MyCanvasGroups.CanvasGroupActivity(LobbyTab_CG, false);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomButtonTab, false);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region NetworkManager_OnRoomCreated
    void NetworkManager_OnRoomCreated()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, false);
    }
    #endregion

    #region OnBackFromCreateRoomTab
    public void OnBackFromCreateRoomTab()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, false);
        MyCanvasGroups.CanvasGroupActivity(LobbyTab_CG, true);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomButtonTab, true);
    }
    #endregion
}
