using Photon.Pun;
using UnityEngine;

public class PlayerComponents : MonoBehaviourPun
{
    public static PlayerComponents instance;

    [Header("COMPONENTS")]
    [SerializeField] PlayerSerializeView playerSerializeView;
    [SerializeField] PlayerEvents playerEvents;
    [SerializeField] PlayerGamePlayStatus playerGamePlayStatus;
    [SerializeField] GetGameManagerEvents getGameManagerEvents;
    [SerializeField] PlayerSelfTimer playerSelfTimer;
    [SerializeField] PlayerLocalView playerLocalView;
    [SerializeField] GetTeamsUI getTeamsUI;
    [SerializeField] PlayerRPC playerRPC;
    [SerializeField] TemporaryDatas temporaryDatas;
    [SerializeField] SetPlayersRoleAsMasterClient setPlayersRoleAsMasterClient;

    public PlayerSerializeView PlayerSerializeView => playerSerializeView;
    public PlayerEvents PlayerEvents => playerEvents;
    public PlayerGamePlayStatus PlayerGamePlayStatus => playerGamePlayStatus;
    public GetGameManagerEvents GetGameManagerEvents => getGameManagerEvents;
    public PlayerSelfTimer PlayerSelfTimer => playerSelfTimer;
    public PlayerLocalView PlayerLocalView => playerLocalView;
    public GetTeamsUI GetTeamsUI => getTeamsUI;
    public PlayerRPC PlayerRPC => playerRPC;
    public TemporaryDatas TemporaryDatas => temporaryDatas;
    public SetPlayersRoleAsMasterClient SetPlayersRoleAsMasterClient => setPlayersRoleAsMasterClient;



    void Awake()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            instance = this;
        }
        else
        {
            enabled = false;
        }
    }







}
