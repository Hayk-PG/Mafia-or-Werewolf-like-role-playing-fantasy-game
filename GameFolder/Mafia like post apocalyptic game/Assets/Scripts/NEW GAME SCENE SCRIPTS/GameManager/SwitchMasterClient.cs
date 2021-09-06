using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SwitchMasterClient : MonoBehaviourPunCallbacks
{
    GameStartAnnouncement _GameStartAnnouncement;
    GameManagerStartTheGame _GameManagerStartTheGame;
    GameManagerPlayerVotesController _GameManagerPlayerVotesController;


    void Awake()
    {
        _GameStartAnnouncement = GetComponent<GameStartAnnouncement>();
        _GameManagerStartTheGame = GetComponent<GameManagerStartTheGame>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus) _SwitchMasterClient();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) _SwitchMasterClient();
    }

    void OnApplicationQuit()
    {
        _SwitchMasterClient();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _SwitchMasterClient();
    }

    void _SwitchMasterClient()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    if (PhotonNetwork.PlayerList.Length > 1)
        //    {
        //        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer.GetNext());
        //    }

        //    PhotonNetwork.SendAllOutgoingCommands();
        //}
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        GameObject MasterTagObj = newMasterClient.TagObject as GameObject;

        _GameStartAnnouncement.OnMasterSwitchedOrRejoined(MasterTagObj.GetComponent<PhotonView>().IsMine);
        _GameManagerStartTheGame.OnMasterSwitchedOrRejoined(MasterTagObj.GetComponent<PhotonView>().IsMine);
    }

    
}
