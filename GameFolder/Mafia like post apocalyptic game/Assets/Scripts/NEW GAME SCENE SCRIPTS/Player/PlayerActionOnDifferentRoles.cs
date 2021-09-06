using Photon.Pun;
using System;

public class PlayerActionOnDifferentRoles: MonoBehaviourPun
{   
    internal void PlayerActionInNightPhase(bool CanPlayerBeActiveInNightPhase, bool HasPlayerVoted, RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInNightPhase)
        {
            if (!HasPlayerVoted)
            {
                photonView.RPC("NightRPC", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, true);
            }

            _RoleButtonController.VoteFXActivity(false, true);
            _RoleButtonController.VoteFXActivityForAllRoleButton(false);
        }  
    }

    internal void PlayerActionInDayPhase(bool CanPlayerBeActiveInDayPhase, bool HasPlayerVoted, RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInDayPhase)
        {
            if (!HasPlayerVoted)
            {
                _RoleButtonController._UI.VotesCount++;

                photonView.RPC("DayVoteRPC", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, false);
            }

            _RoleButtonController.VoteFXActivity(false, true);
            _RoleButtonController.VoteFXActivityForAllRoleButton(false);
        }
    }

    [PunRPC]
    void NightRPC(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        switch (PlayerBaseConditions.PlayerRoleName(senderActorNumber))
        {
            case RoleNames.Medic: OnMedic(votedAgainstActorNumber); break;
            case RoleNames.Sheriff: OnSheriff(votedAgainstActorNumber); break;
            case RoleNames.Infected: OnInfecteds(votedAgainstActorNumber); break;
        }
        
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
    }

    [PunRPC]
    void DayVoteRPC(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        SendPlayerVoteResultToMasterClient(votedAgainstActorNumber);
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
        InformMasterClientAgainstWhomPlayerVoted(votedAgainstActorNumber, senderActorNumber);
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

    void SendPlayerVoteResultToMasterClient(int votedAgainstActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst[votedAgainstActorNumber]++;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.Add(votedAgainstActorNumber, 1);
        }
    }

    void InformMasterClientAgainstWhomPlayerVoted(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted.ContainsKey(senderActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted[senderActorNumber] = Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, RoleButton => RoleButton._OwnerInfo.OwnerActorNumber == votedAgainstActorNumber)._OwnerInfo.OwnerName;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted.Add(senderActorNumber, Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, RoleButton => RoleButton._OwnerInfo.OwnerActorNumber == votedAgainstActorNumber)._OwnerInfo.OwnerName);
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
}
