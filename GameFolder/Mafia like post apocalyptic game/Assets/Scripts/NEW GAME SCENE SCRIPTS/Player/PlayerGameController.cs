using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameController : MonoBehaviourPun, IPlayerGameController
{
    static PlayerGameController MyView;

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
    public bool HasPlayerVotedInNightPhase
    {
        get => _Conditions.hasPlayerVotedInNightPhase;
        set => _Conditions.hasPlayerVotedInNightPhase = value;
    }
    public bool HasPlayerVotedInDayPhase
    {
        get => _Conditions.hasPlayerVotedInDayPhase;
        set => _Conditions.hasPlayerVotedInDayPhase = value;
    }
    public bool HasVotePhaseResetted
    {
        get => _Conditions.hasVotePhaseResetted;
        set => _Conditions.hasVotePhaseResetted = value;
    }


    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;


    void Awake()
    {
        if (PhotonView.IsMine)
        {
            MyView = this;
            _GameManagerSetPlayersRoles = FindObjectOfType<GameManagerSetPlayersRoles>();
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
            if (MyView != null)
            {
                OnNightVote(RoleButtonController);
                OnDayVote(RoleButtonController);
            }
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

    #region OnNightVote + NightPhaseVoting RPC
    void OnNightVote(RoleButtonController RoleButtonController)
    {
        if (CanPlayerBeActiveInNightPhase && !HasPlayerVotedInNightPhase)
        {
            if (RoleButtonController._GameObjects.IconObjs[0].activeInHierarchy)
            {
                MyView.photonView.RPC("NightPhaseVoting", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, RoleButtonController._OwnerInfo.OwnerActorNumber, RoleButtonController._OwnerInfo.OwenrUserId);

                RoleButtonController.GameobjectActivityForAllRoleButtons(0, false);
            }
        }
    }

    [PunRPC]
    void NightPhaseVoting(int senderActorNumber, int votedAgainstActorNumber, string votedAgainstownerId)
    {
        GameObject sender = PhotonNetwork.CurrentRoom.GetPlayer(senderActorNumber).TagObject as GameObject;

        sender.GetComponent<IPlayerGameController>().HasPlayerVotedInNightPhase = true;
    }
    #endregion

    #region OnDayVote + DayPhaseVoting RPC
    void OnDayVote(RoleButtonController RoleButtonController)
    {
        if (CanPlayerBeActiveInDayPhase && !HasPlayerVotedInDayPhase)
        {
            if (RoleButtonController._GameObjects.IconObjs[0].activeInHierarchy)
            {
                MyView.photonView.RPC("DayPhaseVoting", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, RoleButtonController._OwnerInfo.OwnerActorNumber, RoleButtonController._OwnerInfo.OwenrUserId);

                RoleButtonController.GameobjectActivityForAllRoleButtons(0, false);
            }
        }
    }

    [PunRPC]
    void DayPhaseVoting(int senderActorNumber, int votedAgainstActorNumber, string votedAgainstownerId)
    {
        GameObject sender = PhotonNetwork.CurrentRoom.GetPlayer(senderActorNumber).TagObject as GameObject;

        sender.GetComponent<IPlayerGameController>().HasPlayerVotedInDayPhase = true;
    }
    #endregion








}
