using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup playfab_CG;
    [SerializeField] CanvasGroup signUpInTab_CG;
    [SerializeField] CanvasGroup signUpTab_CG;
    [SerializeField] CanvasGroup signInTab_CG;
    [SerializeField] CanvasGroup createOrFind_CG;
    [SerializeField] CanvasGroup createRoomTab_CG;
    [SerializeField] CanvasGroup lobbyTab_CG;

    [Header("BACKGROUND")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite normalBackground;
    [SerializeField] Sprite bluredBackground;

    [Header("UI GAMEOBJECTS")]
    [SerializeField] PlayerBadgeButton playerBadgeButton;

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
    public CanvasGroup CreateOrFindTab => createOrFind_CG;
    public CanvasGroup CreateRoomTab_CG => createRoomTab_CG;
    public CanvasGroup LobbyTab_CG => lobbyTab_CG;

    public PlayerBadgeButton PlayerBadgeButton => playerBadgeButton;


    void OnEnable()
    {
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomCreated += NetworkManager_OnRoomCreated;
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomJoined += NetworkManager_OnRoomJoined;

        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickSignUpInButton += NetworkUIButtons_OnClickSignUpInButton;
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickCreateButton += NetworkUIButtons_OnClickCreateButton;
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickFindButton += NetworkUIButtons_OnClickFindButton;       
    }
    
    void OnDisable()
    {
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickSignUpInButton -= NetworkUIButtons_OnClickSignUpInButton;
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomCreated -= NetworkManager_OnRoomCreated;
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomJoined -= NetworkManager_OnRoomJoined;

        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickCreateButton -= NetworkUIButtons_OnClickCreateButton;
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickFindButton -= NetworkUIButtons_OnClickFindButton;
    }

    #region OnLoggedIn
    public void OnLoggedIn()
    {
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, false);
        MyCanvasGroups.CanvasGroupActivity(CreateOrFindTab, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region OnLoggedOut
    public void OnLoggedOut()
    {       
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, true);
        MyCanvasGroups.CanvasGroupActivity(SignUpInTab, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region OnLoginToAnotherAccount
    public void OnLoginToAnotherAccount()
    {
        MyCanvasGroups.CanvasGroupActivity(PlayfabTab, true);
        MyCanvasGroups.CanvasGroupActivity(SignInTab, true);

        BackgroundImage.sprite = bluredBackground;
    }
    #endregion

    #region OnBackToMainMenu
    public void OnBackToMainMenu()
    {
        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            MyCanvasGroups.CanvasGroupActivity(CreateOrFindTab, true);
        }
        else
        {
            OnLoggedOut();          
        }
    }
    #endregion

    #region NetworkUIButtons_OnClickSignUpInButton
    void NetworkUIButtons_OnClickSignUpInButton(bool isSignUpButtonPressed)
    {
        MyCanvasGroups.CanvasGroupActivity(SignUpInTab, false);

        if (isSignUpButtonPressed)
        {
            MyCanvasGroups.CanvasGroupActivity(SignUpTab, true);
        }
        else
        {
            MyCanvasGroups.CanvasGroupActivity(SignInTab, true);
        }
    }
    #endregion

    #region NetworkUIButtons_OnClickCreateButton
    void NetworkUIButtons_OnClickCreateButton()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateOrFindTab, false);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, true);
    }
    #endregion

    #region NetworkManager_OnRoomCreated
    void NetworkManager_OnRoomCreated()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, false);
    }
    #endregion

    #region NetworkUIButtons_OnClickFindButton
    void NetworkUIButtons_OnClickFindButton()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateOrFindTab, false);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, false);
        MyCanvasGroups.CanvasGroupActivity(LobbyTab_CG, true);
    }
    #endregion

    #region NetworkManager_OnRoomJoined
    void NetworkManager_OnRoomJoined()
    {
        MyCanvasGroups.CanvasGroupActivity(CreateOrFindTab, false);
        MyCanvasGroups.CanvasGroupActivity(CreateRoomTab_CG, false);
        MyCanvasGroups.CanvasGroupActivity(LobbyTab_CG, false);
    }
    #endregion 
    







}
