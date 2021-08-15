using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayersRoleAsMasterClient : MonoBehaviourPun
{
    public static SetPlayersRoleAsMasterClient Master;

    [Header("ROLE INDEX")]
    [SerializeField] int playerIndex;
    [SerializeField] int roleIndex;
    [SerializeField] int playersCount;
    [SerializeField] int gendetRandomNumber;

    GlobalInputs GlobalInputs;

    public int PlayerIndex
    {
        get
        {
            return playerIndex;
        }
        set
        {
            playerIndex = value;
        }
    }
    public int RoleIndex
    {
        get
        {
            return roleIndex;
        }
        set
        {
            roleIndex = value;
        }
    }
    public int PlayersCount
    {
        get
        {
            return playersCount;
        }
        set
        {
            playersCount = value;
        }
    }
    public int GenderRandomNumber
    {
        get
        {
            return gendetRandomNumber;
        }
        set
        {
            gendetRandomNumber = value;
        }
    }

    List<int> PlayerRolesIndex = new List<int>();

    [Header("COROUTINE STAGES TRANSITION TIME")]
    [SerializeField] float stageTwoTransitionTime;
    [SerializeField] float stageThreeTransitionTime;


    void Awake()
    {
        if (PlayerBaseConditions._PlayerIsMasterClient(GetComponent<PhotonView>().OwnerActorNr))
        {
            Master = this;           
        }
        else
        {
            enabled = false;
        }

        GlobalInputs = FindObjectOfType<GameControllerComponents>().GlobalInputs;
    }

    void Start()
    {
        ShowGameStartButton();
    }

    void OnEnable()
    {
        if (GlobalInputs != null)
        {
            GlobalInputs.OnClickGameStartButton += GlobalInputs_OnClickGameStartButton;
        }
    }
    
    void OnDisable()
    {
        if (GlobalInputs != null)
        {
            GlobalInputs.OnClickGameStartButton -= GlobalInputs_OnClickGameStartButton;
        }
    }

    #region OnMasterSwitched
    public void OnMasterSwitched()
    {
        Master = this;

        GlobalInputs.OnClickGameStartButton -= GlobalInputs_OnClickGameStartButton;

        PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnClickGameStartButton += GlobalInputs_OnClickGameStartButton; 

        ShowGameStartButton();      
    }
    #endregion

    #region ShowGameStartButton
    void ShowGameStartButton()
    {
        FindObjectOfType<GameUI>().ShowGameStartButtonToMasterClient(Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);
    }
    #endregion

    #region GlobalInputs_OnClickGameStartButton
    void GlobalInputs_OnClickGameStartButton(int actorNumber)
    {
        if (PlayerBaseConditions._PlayerIsMasterClient(actorNumber))
        {
            StartCoroutine(CoroutineToSendInfoToAllPlayers());
        }
    }
    #endregion

    #region Set players roles as a master client and start the game
    IEnumerator CoroutineToSendInfoToAllPlayers()
    {
        PlayersCount = PhotonNetwork.PlayerList.Length;

        GetRolesCount();

        yield return null;

        //Stage 1
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ISetPlayerRolePropsRPC(i);

            yield return null;
        }
       
        yield return new WaitForSeconds(stageTwoTransitionTime);

        //Stage 2
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            NotifyPlayersToAssigneTheirAvatarButtonsRPC(i);

            yield return null;
        }

        yield return new WaitForSeconds(stageThreeTransitionTime);

        //Stage 3
        GameControllerComponents.instance.GameStart.StartTheGame();
    }

    #region Call RPCs
    void GetRolesCount()
    {
        for (int i = 0; i < PlayersCount; i++)
        {
            PlayerRolesIndex.Add(i);
        }
    }

    void ISetPlayerRolePropsRPC(int i)
    {
        PlayerIndex = i;
        RoleIndex = PlayerRolesIndex[Random.Range(0, PlayerRolesIndex.Count - 1)];

        Master.photonView.RPC("SetPlayersRoles", RpcTarget.All, PlayerIndex, RoleIndex);

        PlayerRolesIndex.Remove(RoleIndex);
    }

    void NotifyPlayersToAssigneTheirAvatarButtonsRPC(int i)
    {
        int playerIndex = i;

        Master.photonView.RPC("SetPlayersSetOwnedAvatarButtonSpriteTrue", RpcTarget.All, playerIndex);
    }
    #endregion

    #region RPCs

    [PunRPC]
    void SetPlayersRoles(int PlayerIndex, int RoleIndex)
    {
        GameObject playerObj = (GameObject)PhotonNetwork.PlayerList[PlayerIndex].TagObject;
        playerObj.GetComponent<ISetPlayerRoleProps>().AvatarButtonIndex = PlayerIndex;
        playerObj.GetComponent<ISetPlayerRoleProps>().RoleNumber = RoleIndex;
        playerObj.GetComponent<ISetPlayerRoleProps>().TakeAvatarButtonOwnership = true;
        playerObj.GetComponent<ISetPlayerRoleProps>().RoleName = GameControllerComponents.instance.InstantiatePlayers.PlayersRolesNames[RoleIndex];
    }

    [PunRPC]
    void SetPlayersSetOwnedAvatarButtonSpriteTrue(int playerIndex)
    {
        GameObject playerObj = (GameObject)PhotonNetwork.PlayerList[playerIndex].TagObject;
        playerObj.GetComponent<ISetPlayerRoleProps>().SetOwnedAvatarButtonSprite = true;
    }
    #endregion

    #endregion











}
