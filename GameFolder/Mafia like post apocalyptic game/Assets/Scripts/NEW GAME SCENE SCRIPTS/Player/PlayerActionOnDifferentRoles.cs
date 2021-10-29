using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActionOnDifferentRoles: MonoBehaviourPun
{
    [Serializable] struct VFXprefabs
    {
        [SerializeField] internal GameObject[] VFX;
    }
    [SerializeField] VFXprefabs _VFXprefabs;

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
                photonView.RPC("NightInLocalViewRPC", RpcTarget.All, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber);

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
            case RoleNames.Medic:
                OnMedic(votedAgainstActorNumber);
                _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheDoctor, IsInfected(votedAgainstActorNumber) ? -25 : 125);
                break;

            case RoleNames.Sheriff:
                OnSheriff(votedAgainstActorNumber);
                _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheSheriff, IsInfected(votedAgainstActorNumber) ? -25 : 105);
                break;

            case RoleNames.Infected:
                OnInfecteds(votedAgainstActorNumber);
                _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheInfected, IsInfected(votedAgainstActorNumber) ? -25 : 75);
                break;

            case RoleNames.Soldier:
                OnSoldier(votedAgainstActorNumber);
                _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheSoldier, IsInfected(votedAgainstActorNumber) ? -25 : 75);
                break;

            case RoleNames.Lizard:
                OnLizard(votedAgainstActorNumber);
                _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsOfTheLizard, IsInfected(votedAgainstActorNumber) ? -25 : 125);
                break;
        }
        
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
    }

    [PunRPC]
    void NightInLocalViewRPC(int votedAgainstActorNumber, int senderActorNumber)
    {
        switch (PlayerBaseConditions.PlayerRoleName(senderActorNumber))
        {
            case RoleNames.Medic:
                photonView.RPC("CreateHealerVFX", RpcTarget.All, votedAgainstActorNumber, senderActorNumber);
                break;

            case RoleNames.Sheriff:
                photonView.RPC("DiscoverTheRoleVFX", RpcTarget.All, votedAgainstActorNumber, senderActorNumber);
                break;

            case RoleNames.Infected:
                photonView.RPC("OrcAttackVFX", RpcTarget.All, votedAgainstActorNumber, senderActorNumber);
                break;

            case RoleNames.Soldier:
                photonView.RPC("KnightAttackVFX", RpcTarget.All, votedAgainstActorNumber, senderActorNumber);
                break;

            case RoleNames.Lizard:
                photonView.RPC("WitchVFX", RpcTarget.All, votedAgainstActorNumber, senderActorNumber);
                break;
        }
    }

    [PunRPC]
    void DayVoteRPC(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        SendPlayerVoteResultToMasterClient(votedAgainstActorNumber, senderActorNumber);
        PlayerVoted(votedAgainstActorNumber, senderActorNumber, isNightPhase);
        InformMasterClientAgainstWhomPlayerVoted(votedAgainstActorNumber, senderActorNumber);
        _GameManagerTimer.AddOrRemovePoints(senderActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, IsInfected(senderActorNumber) != IsInfected(votedAgainstActorNumber) ? UnityEngine.Random.Range(75, 150) : -50);
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
  
    [PunRPC]
    void CreateHealerVFX(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (Photonview(senderActorNumber).IsMine && Photonview(senderActorNumber).AmOwner )
        {
            GameObject healerVFX = Instantiate(_VFXprefabs.VFX[0], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 490.7871f), Quaternion.identity);
        }
        if (Photonview(votedAgainstActorNumber).IsMine && Photonview(votedAgainstActorNumber).AmOwner)
        {
            GameObject healerVFX = Instantiate(_VFXprefabs.VFX[0], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 490.7871f), Quaternion.identity);
        }
    }

    [PunRPC]
    void OrcAttackVFX(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (Photonview(senderActorNumber).IsMine && Photonview(senderActorNumber).AmOwner)
        {
            GameObject orcAttackVfx = Instantiate(_VFXprefabs.VFX[1], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 0), Quaternion.identity);
        }
        if (Photonview(votedAgainstActorNumber).IsMine && Photonview(votedAgainstActorNumber).AmOwner)
        {
            GameObject negativeEffectVfx = Instantiate(_VFXprefabs.VFX[2], Vector3.zero, Quaternion.identity);
        }
    }

    [PunRPC]
    void WitchVFX(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (Photonview(senderActorNumber).IsMine && Photonview(senderActorNumber).AmOwner)
        {
            GameObject witchVfx = Instantiate(_VFXprefabs.VFX[3], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, -402.1837f), Quaternion.identity);
        }
        if (Photonview(votedAgainstActorNumber).IsMine && Photonview(votedAgainstActorNumber).AmOwner)
        {
            GameObject witchVfx = Instantiate(_VFXprefabs.VFX[3], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, -402.1837f), Quaternion.identity);
        }
    }

    [PunRPC]
    void KnightAttackVFX(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (Photonview(senderActorNumber).IsMine && Photonview(senderActorNumber).AmOwner)
        {
            GameObject knightAttackVfx = Instantiate(_VFXprefabs.VFX[4], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 0f), Quaternion.identity);
        }
        if (Photonview(votedAgainstActorNumber).IsMine && Photonview(votedAgainstActorNumber).AmOwner)
        {
            GameObject knightAttackVfx = Instantiate(_VFXprefabs.VFX[4], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 0f), Quaternion.identity);
        }      
    }

    [PunRPC]
    void DiscoverTheRoleVFX(int votedAgainstActorNumber, int senderActorNumber)
    {
        if (Photonview(senderActorNumber).IsMine && Photonview(senderActorNumber).AmOwner)
        {
            GameObject discoverTheRoleVfx = Instantiate(_VFXprefabs.VFX[5], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 0f), Quaternion.identity);

            RoleButtonController discoveredPlayer = PlayerBaseConditions.GetRoleButton(votedAgainstActorNumber);
            discoveredPlayer._UI.VisibleToEveryoneImage = discoveredPlayer._UI.RoleImage;
        }
        if (Photonview(votedAgainstActorNumber).IsMine && Photonview(votedAgainstActorNumber).AmOwner)
        {
            GameObject eyeNegativeEffectVfx = Instantiate(_VFXprefabs.VFX[6], new Vector3(RoleButtonTransform(votedAgainstActorNumber).x, RoleButtonTransform(votedAgainstActorNumber).y, 0f), Quaternion.identity);
        }
    }

    bool IsInfected(int votedAgainstActorNumber)
    {
        return PlayerBaseConditions.GetRoleButton(votedAgainstActorNumber)._GameInfo.RoleName == RoleNames.Infected || PlayerBaseConditions.GetRoleButton(votedAgainstActorNumber)._GameInfo.RoleName == RoleNames.Lizard;
    }

    PhotonView Photonview(int actorNumber)
    {
        return PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PhotonView>();
    }
    Vector3 RoleButtonTransform(int actorNumber)
    {
        return PlayerBaseConditions.GetRoleButton(actorNumber).transform.position;
    }
}
