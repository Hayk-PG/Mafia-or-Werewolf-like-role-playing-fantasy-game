using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerGetLostPlayers : MonoBehaviourPun
{
    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate 
        {
            PlayerBaseConditions._MyGameManager.OnCalculatePlayerVotes += _MyGameManager_OnCalculatePlayerVotes;
        });
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnCalculatePlayerVotes -= _MyGameManager_OnCalculatePlayerVotes;
        }
    }

    void _MyGameManager_OnCalculatePlayerVotes()
    {
        PlayerBaseConditions._MyGameManager.photonView.RPC("GetLostPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
    }











}
