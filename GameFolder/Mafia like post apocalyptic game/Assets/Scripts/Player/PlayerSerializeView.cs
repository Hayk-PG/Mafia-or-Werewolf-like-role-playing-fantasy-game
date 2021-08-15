using Photon.Pun;
using UnityEngine;

public class PlayerSerializeView : MonoBehaviourPun, IPunObservable,ISetPlayerRoleProps
{
    [Header("COMPONENTS")]
    [SerializeField] PlayerSelfTimer playerSelfTimer;
    [SerializeField] SetPlayersRoleAsMasterClient masterClient;
    [SerializeField] PlayerGamePlayStatus playerGamePlayStatus;

    #region ISetPlayerRoleProps
    [Header("ISetPlayerRoleProps")]
    [SerializeField] int genderRadomNumber;
    [SerializeField] int roleNumber;
    [SerializeField] int avatarButtonsIndex;    
    [SerializeField] string roleName;
    [SerializeField] bool takeAvatarButtonOwnership;
    [SerializeField] bool setOwnedAvatarButtonSprite;

    public int GenderRandomNumber
    {
        get
        {
            return genderRadomNumber;
        }
        set
        {
            genderRadomNumber = value;
        }
    }
    public int RoleNumber
    {
        get
        {
            return roleNumber;
        }
        set
        {
            roleNumber = value;
        }
    }
    public int AvatarButtonIndex
    {
        get
        {
            return avatarButtonsIndex;
        }
        set
        {
            avatarButtonsIndex = value;
        }
    }  
    public string RoleName
    {
        get
        {
            return roleName;
        }
        set
        {
            roleName = value;
        }
    }
    public bool TakeAvatarButtonOwnership
    {
        get
        {
            return takeAvatarButtonOwnership;
        }
        set
        {
            takeAvatarButtonOwnership = value;
        }
    }
    public bool SetOwnedAvatarButtonSprite
    {
        get
        {
            return setOwnedAvatarButtonSprite;
        }
        set
        {
            setOwnedAvatarButtonSprite = value;
        }
    }
    #endregion

    #region PlayerGamePlayStatus
    PlayerGamePlayStatus PlayerGamePlayStatus => playerGamePlayStatus;
    #endregion


    void Awake()
    {
        GenderRandomNumber = Random.Range(0, 2);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Timer
            stream.SendNext(playerSelfTimer.Second);
            stream.SendNext(playerSelfTimer.NightsCount);
            stream.SendNext(playerSelfTimer.DaysCount);
            stream.SendNext(playerSelfTimer.IsNight);

            //masterClient
            stream.SendNext(masterClient.PlayerIndex);
            stream.SendNext(masterClient.RoleIndex);
            stream.SendNext(masterClient.PlayersCount);

            //ISetPlayerRoleProps
            stream.SendNext(GenderRandomNumber);
            stream.SendNext(RoleNumber);
            stream.SendNext(AvatarButtonIndex);           
            stream.SendNext(RoleName);
            stream.SendNext(TakeAvatarButtonOwnership);
            stream.SendNext(SetOwnedAvatarButtonSprite);

            //PlayerGamePlayStatus
            stream.SendNext(PlayerGamePlayStatus.CanPlayerVote);
            stream.SendNext(PlayerGamePlayStatus.IsPlayerStillPlaying);
            stream.SendNext(PlayerGamePlayStatus.VotesCountThatPlayerGot);
            stream.SendNext(PlayerGamePlayStatus.GotSaved);
            stream.SendNext(PlayerGamePlayStatus.GotDiscovered);
            stream.SendNext(PlayerGamePlayStatus.GotCompromised);
        }
        else
        {
            //Timer
            playerSelfTimer.Second = (byte)stream.ReceiveNext();
            playerSelfTimer.NightsCount = (byte)stream.ReceiveNext();
            playerSelfTimer.DaysCount = (byte)stream.ReceiveNext();
            playerSelfTimer.IsNight = (bool)stream.ReceiveNext();

            //masterClient
            masterClient.PlayerIndex = (int)stream.ReceiveNext();
            masterClient.RoleIndex = (int)stream.ReceiveNext();
            masterClient.PlayersCount = (int)stream.ReceiveNext();

            //ISetPlayerRoleProps
            GenderRandomNumber = (int)stream.ReceiveNext();
            RoleNumber = (int)stream.ReceiveNext();
            AvatarButtonIndex = (int)stream.ReceiveNext();           
            RoleName = (string)stream.ReceiveNext();
            TakeAvatarButtonOwnership = (bool)stream.ReceiveNext();
            SetOwnedAvatarButtonSprite = (bool)stream.ReceiveNext();

            //PlayerGamePlayStatus
            PlayerGamePlayStatus.CanPlayerVote = (bool)stream.ReceiveNext();
            PlayerGamePlayStatus.IsPlayerStillPlaying = (bool)stream.ReceiveNext();
            PlayerGamePlayStatus.VotesCountThatPlayerGot = (int)stream.ReceiveNext();
            PlayerGamePlayStatus.GotSaved = (bool)stream.ReceiveNext();
            PlayerGamePlayStatus.GotDiscovered = (bool)stream.ReceiveNext();
            PlayerGamePlayStatus.GotCompromised = (bool)stream.ReceiveNext();
        }
    }
















}
