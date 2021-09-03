using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameController : MonoBehaviourPun, IPlayerGameController
{
    [Serializable] class Conditions
    {
        [SerializeField] internal bool canPlayerBeActiveInNightPhase;
        [SerializeField] internal bool canPlayerBeActiveInDayPhase;
        [SerializeField] internal bool isPlayerAlive;
        [SerializeField] internal bool hasPlayerVotedInNightPhase;
        [SerializeField] internal bool hasPlayerVotedInDayPhase;
        [SerializeField] internal bool hasVotePhaseResetted;
    }
    [SerializeField] Conditions _Conditions;

    public PhotonView PhotonView
    {
        get => GetComponent<PhotonView>();
    }
    public bool CanPlayerBeActiveInNightPhase
    {
        get => _Conditions.canPlayerBeActiveInNightPhase;
        set => _Conditions.canPlayerBeActiveInNightPhase = value;
    }
    public bool CanPlayerBeActiveInDayPhase
    {
        get => _Conditions.canPlayerBeActiveInDayPhase;
        set => _Conditions.canPlayerBeActiveInDayPhase = value;
    }
    public bool IsPlayerAlive
    {
        get => _Conditions.isPlayerAlive;
        set => _Conditions.isPlayerAlive = value;
    }    
    public bool HasVotePhaseResetted
    {
        get => _Conditions.hasVotePhaseResetted;
        set => _Conditions.hasVotePhaseResetted = value;
    }


    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;
    GameManagerPlayerVotesController _GameManagerPlayerVotesController;


    void Awake()
    {
        if (photonView.IsMine)
        {
            _GameManagerSetPlayersRoles = FindObjectOfType<GameManagerSetPlayersRoles>();
            _GameManagerPlayerVotesController = FindObjectOfType<GameManagerPlayerVotesController>();
        }
        else
        {
            enabled = false;
        }
        
    }

    void Update()
    {
        RoleButtonPressed(RoleButtonController =>
        {
            OnNightVote(RoleButtonController);
            OnDayVote(RoleButtonController);
        });
    }

    void RoleButtonPressed(Action<RoleButtonController> OnClick)
    {
        foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            roleButton.GetComponent<Button>().onClick.RemoveAllListeners();
            roleButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClick?.Invoke(roleButton);
            });
        }
    }

    #region OnNightVote
    void OnNightVote(RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInNightPhase)
        {
            bool hasPlayerVoted = _GameManagerPlayerVotesController._Votes.PlayerVoteCondition.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber) ?
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[PhotonNetwork.LocalPlayer.ActorNumber][0] : false;

            if (!hasPlayerVoted)
            {               
                _RoleButtonController._UI.VotesCount++;
                
                photonView.RPC("SendVotingInformationToMaster", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, true);

                _RoleButtonController.GameobjectActivityForAllRoleButtons(0, false);
            }
        }
    }
    #endregion

    #region OnDayVote
    void OnDayVote(RoleButtonController _RoleButtonController)
    {
        if (CanPlayerBeActiveInDayPhase)
        {
            bool hasPlayerVoted = _GameManagerPlayerVotesController._Votes.PlayerVoteCondition.ContainsKey(PhotonNetwork.LocalPlayer.ActorNumber) ?
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[PhotonNetwork.LocalPlayer.ActorNumber][1] : false;

            if (!hasPlayerVoted)
            {
                _RoleButtonController._UI.VotesCount++;
               
                photonView.RPC("SendVotingInformationToMaster", RpcTarget.MasterClient, _RoleButtonController._OwnerInfo.OwnerActorNumber, PhotonNetwork.LocalPlayer.ActorNumber, false);

                _RoleButtonController.GameobjectActivityForAllRoleButtons(0, false);
            }
        }
    }
    #endregion

    #region SendVotingInformationToMaster RPC
    [PunRPC]
    void SendVotingInformationToMaster(int votedAgainstActorNumber, int senderActorNumber, bool isNightPhase)
    {
        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.ContainsKey(votedAgainstActorNumber))
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst[votedAgainstActorNumber]++;
        }
        else
        {
            FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst.Add(votedAgainstActorNumber, 1);
        }


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
    #endregion
}
