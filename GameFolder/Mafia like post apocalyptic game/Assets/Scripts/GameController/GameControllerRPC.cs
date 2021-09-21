using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;


public class GameControllerRPC : MonoBehaviourPun
{
    public event Action<Player> OnLostPlayer;

    #region OnClickGameStartButtonRPC RPC
    [PunRPC]
    public void OnClickGameStartButtonRPC()
    {
        PlayerBaseConditions._MyGameControllerComponents.GameUI.CanvasGroupActivity(GetComponent<GameControllerComponents>().GameUI.GameStartButton, false);
    }
    #endregion

    #region OnStartTheGame RPC
    [PunRPC]
    public void OnStartTheGame()
    {
        PlayerBaseConditions._MyGameControllerComponents.GameUI.CanvasGroupActivity(GetComponent<GameControllerComponents>().GameUI.GameStartTab, false);
        PlayerBaseConditions._MyGameControllerComponents.GameStart.IsGameStarted = true;

        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(0);

        string text = "<b>" + FindObjectOfType<ChatController>()._GameObjects.ChatContainer.childCount + ") " + "<PAUTIK>" + "</b>" + "\n" + "Good luck!";
        FindObjectOfType<ChatController>().InstantiateChatText(text, new Color32(242, 255, 0, 255), new Color32(255, 19, 0, 50), 1);

        //PlayerBaseConditions._MyGameControllerComponents.TextFX.LetsBeginTextObj.SetActive(true);
    }
    #endregion

    #region GetMostCorrectTime RPC
    [PunRPC]
    public void GetMostCorrectTime(int actorNumber)
    {
        if(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber) != null)
        {
            GameObject player = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;

            if(player != null)
            {
                SyncPlayersTimer.GSync.MostSyncedPlayerTimer = player.GetComponent<PlayerSelfTimer>();
            }           
        }     
    }
    #endregion

    #region GetLostPlayer RPC

    [PunRPC]
    public void GetLostPlayer(int actorNumber)
    {
        if (PlayerBaseConditions._PlayerIsMasterClient(actorNumber))
        {
            List<int> PlayersVotesList = new List<int>();
            List<int> PlayersActrNmbrList = new List<int>();

            AddPlayersParamsToLists(PlayersVotesList, PlayersActrNmbrList);

            SortTheLists(PlayersVotesList, PlayersActrNmbrList);

            if (PlayersVotesList.Count >= 2)
            {
                if (PlayersVotesList[PlayersVotesList.Count - 1] > 0)
                {
                    if (PlayersVotesList[PlayersVotesList.Count - 1] > PlayersVotesList[PlayersVotesList.Count - 2])
                    {
                        ChooseTheSingleHighVotePlayer(PlayersVotesList);
                    }
                    else
                    {
                        ChooseOneFromMultiplyHightVotePlayers(PlayersVotesList, PlayersActrNmbrList);
                    }
                }
            }
           
            MakeZeroPlayersVotesCount();
        }
    }

    void AddPlayersParamsToLists(List<int> PlayersVotesList, List<int> PlayersActrNmbrList)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            PlayersVotesList.Add(playerObj.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot);
            PlayersActrNmbrList.Add(playerObj.GetComponent<SetPlayerInfo>().ActorNumber);
        }
    }

    void SortTheLists(List<int> PlayersVotesList, List<int> PlayersActrNmbrList)
    {
        PlayersVotesList.Sort();
        PlayersActrNmbrList.Sort();
    }

    void ChooseTheSingleHighVotePlayer(List<int> PlayersVotesList)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            if (playerObj.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot >= PlayersVotesList[PlayersVotesList.Count - 1])
            {
                photonView.RPC("LetEveryoneKnowWhoLost", RpcTarget.All, playerObj.GetComponent<SetPlayerInfo>().ActorNumber, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }

    void ChooseOneFromMultiplyHightVotePlayers(List<int> PlayersVotesList, List<int> PlayersActrNmbrList)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            if (playerObj.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot >= PlayersVotesList[PlayersVotesList.Count - 1])
            {
                photonView.RPC("LetEveryoneKnowWhoLost", RpcTarget.All, playerObj.GetComponent<SetPlayerInfo>().ActorNumber, PhotonNetwork.LocalPlayer.ActorNumber);

                break;
            }
        }
    }

    void MakeZeroPlayersVotesCount()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            playerObj.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot = 0;
        }
    }

    [PunRPC]
    public void LetEveryoneKnowWhoLost(int actorNumber, int localActorNumber)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(PlayerBaseConditions._PlayerTagObject(localActorNumber).GetComponent<SetPlayerInfo>().ViewID))
        {
            GameObject lostPlayer = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;

            lostPlayer.GetComponent<PlayerGamePlayStatus>().IsPlayerStillPlaying = false;

            OnLostPlayer?.Invoke(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber));
        }       
    }

    #endregion

}
