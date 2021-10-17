using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionOnDifferentRoles: MonoBehaviourPun
{
    GameManagerTimer _GameManagerTimer { get; set; }


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
    }

    internal void PlayerActionInNightPhase(bool CanPlayerBeActiveInNightPhase, RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInNightPhase)
        {
            if (!HasPlayerVoted(PhotonNetwork.LocalPlayer.ActorNumber, true))
            {
                photonView.RPC("NightRPC", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, true);
                photonView.RPC("NightInLocalViewRPC", PhotonNetwork.CurrentRoom.GetPlayer(_RoleButtonController._OwnerInfo.OwnerActorNumber), PhotonNetwork.LocalPlayer.ActorNumber);

                _RoleButtonController.VoteFXActivity(false, true);
                _RoleButtonController.VoteFXActivityForAllRoleButton(false);
                _RoleButtonController._UI.Selected.SetActive(true);
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(11);
            }           
        }  
    }

    internal void PlayerActionInDayPhase(bool CanPlayerBeActiveInDayPhase, RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInDayPhase)
        {
            if (!HasPlayerVoted(PhotonNetwork.LocalPlayer.ActorNumber, false))
            {
                _RoleButtonController._UI.VotesCount++;

                photonView.RPC("DayVoteRPC", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, false);

                _RoleButtonController.VoteFXActivity(false, true);
                _RoleButtonController.VoteFXActivityForAllRoleButton(false);
                _RoleButtonController._UI.Selected.SetActive(true);
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(11);
            }           
        }
    }

    [PunRPC]
    void NightRPC(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        switch (PlayerBaseConditions.PlayerRoleName(senderActorNumber))
        {
            case RoleNames.Medic: OnMedic(votedAgainstActorNumber);  Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheDoctor, IsInfected(votedAgainstActorNumber) ? -25 : UnityEngine.Random.Range(75, 150)); break;
            case RoleNames.Sheriff: OnSheriff(votedAgainstActorNumber); Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheSheriff, IsInfected(votedAgainstActorNumber) ? -25 : UnityEngine.Random.Range(75, 150)); break;
            case RoleNames.Infected: OnInfecteds(votedAgainstActorNumber); Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheInfected, IsInfected(votedAgainstActorNumber) ? -25 : UnityEngine.Random.Range(75, 150)); break;
            case RoleNames.Soldier: OnSoldier(votedAgainstActorNumber); Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheSoldier, IsInfected(votedAgainstActorNumber) ? -25 : UnityEngine.Random.Range(75, 150)); break;
            case RoleNames.Lizard: OnLizard(votedAgainstActorNumber); Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheLizard, IsInfected(votedAgainstActorNumber) ? -25 : UnityEngine.Random.Range(75, 150)); break;
        }
        
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
    }

    [PunRPC]
    void DayVoteRPC(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        SendPlayerVoteResultToMasterClient(votedAgainstActorNumber, senderActorNumber);
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
        InformMasterClientAgainstWhomPlayerVoted(votedAgainstActorNumber, senderActorNumber);
        Points(senderActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, IsInfected(senderActorNumber) != IsInfected(votedAgainstActorNumber) ? UnityEngine.Random.Range(75, 150) : -50);
    }

    [PunRPC]
    void NightInLocalViewRPC(int senderActorNumber)
    {
        if (photonView.IsMine && photonView.AmOwner)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            string roleName = PlayerBaseConditions.PlayerRoleName(senderActorNumber);

            PlayerBaseConditions.GetRoleButton(localPlayer.ActorNumber).OnNightVotesFXInLocalView(roleName);
        }
    }

    int RoleIndex()
    {
        return PlayerBaseConditions.RoleIndex(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    void PlayerVoted(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition.ContainsKey(senderActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition[senderActorNumber][0] = isNightPhase;
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition[senderActorNumber][1] = !isNightPhase;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition.Add(senderActorNumber, new bool[2] { isNightPhase, !isNightPhase });
        }
    }

    bool HasPlayerVoted(int senderActorNumber, bool isNightPhase)
    {
        if(FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition.ContainsKey(senderActorNumber))
        {
            if (isNightPhase)
            {
                return FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition[senderActorNumber][0];
            }
            else
            {
                return FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition[senderActorNumber][1];
            }
        }
        else
        {
            return false;
        }
    }

    void SendPlayerVoteResultToMasterClient(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (HasLizardVotedAgainstUs(senderActorNumber))
        {
            Vote(senderActorNumber);
        }
        else
        {
            Vote(votedAgainstActorNumber);
        }  
    }

    bool HasLizardVotedAgainstUs(int senderActorNumber)
    {
        return FindObjectOfType<GameManagerPlayerVotesController>()._Votes.LizardVoteAgainst.ContainsKey(senderActorNumber);
    }

    void Vote(int actorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.ContainsKey(actorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst[actorNumber]++;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.Add(actorNumber, 1);
        }
    }

    void InformMasterClientAgainstWhomPlayerVoted(int votedAgainstActorNumber, int senderActorNumber)
    {
        GameManagerPlayerVotesController.Votes Votes = FindObjectOfType<GameManagerPlayerVotesController>()._Votes;        
        string newName = Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, RoleButton => RoleButton._OwnerInfo.OwnerActorNumber == votedAgainstActorNumber)._OwnerInfo.OwnerName;
        
        if (Votes.AgainstWhomPlayerVoted.ContainsKey(senderActorNumber))
        {
            List<string> names = Votes.AgainstWhomPlayerVoted[senderActorNumber].ToList();
            names.Add(newName);
            Votes.AgainstWhomPlayerVoted[senderActorNumber] = names.ToArray();
        }
        else
        {
            Votes.AgainstWhomPlayerVoted.Add(senderActorNumber, new string[] { newName });           
        }
    }

    void OnMedic(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.HealedPlayers.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.HealedPlayers[votedAgainstActorNumber] = true;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.HealedPlayers.Add(votedAgainstActorNumber, true);
        }
    }

    void OnSheriff(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.DiscoverTheRole.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.DiscoverTheRole[votedAgainstActorNumber] = true;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.DiscoverTheRole.Add(votedAgainstActorNumber, true);
        }
    }

    void OnInfecteds(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.InfectedVotesAgainst.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.InfectedVotesAgainst[votedAgainstActorNumber]++;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.InfectedVotesAgainst.Add(votedAgainstActorNumber, 1);
        }
    }

    void OnSoldier(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.SoldierVoteAgainst.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.SoldierVoteAgainst[votedAgainstActorNumber]++;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.SoldierVoteAgainst.Add(votedAgainstActorNumber, 1);
        }
    }

    void OnLizard(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.LizardVoteAgainst.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.LizardVoteAgainst[votedAgainstActorNumber] = true;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.LizardVoteAgainst.Add(votedAgainstActorNumber, true);
        }
    }

    void Points(int senderActorNumber, Dictionary<string, int> PointsDict, int point)
    {
        string dictKey = PlayerBaseConditions.GetRoleButton(senderActorNumber)._OwnerInfo.OwenrUserId;
        
        if (PointsDict.ContainsKey(dictKey))
        {
            PointsDict[dictKey] = PointsDict[dictKey] += point;
        }
        else
        {
            PointsDict.Add(dictKey, point);
        }
    }

    bool IsInfected(int votedAgainstActorNumber)
    {
        return PlayerBaseConditions.GetRoleButton(votedAgainstActorNumber)._GameInfo.RoleName == RoleNames.Infected || PlayerBaseConditions.GetRoleButton(votedAgainstActorNumber)._GameInfo.RoleName == RoleNames.Lizard;
    }
}
