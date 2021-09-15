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
    public virtual bool CanPlayerBeActiveInNightPhase
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


    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles { get; set; }
    ProfileForGameScene _ProfileForGameScene { get; set; }
    PlayerActionOnDifferentRoles _PlayerActionOnDifferentRoles { get; set; }


    void Awake()
    {
        if (photonView.IsMine)
        {
            _GameManagerSetPlayersRoles = FindObjectOfType<GameManagerSetPlayersRoles>();
            _ProfileForGameScene = FindObjectOfType<ProfileForGameScene>();
            _PlayerActionOnDifferentRoles = GetComponent<PlayerActionOnDifferentRoles>();
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
            if (!String.IsNullOrEmpty(RoleButtonController._OwnerInfo.OwenrUserId))
            {
                if (RoleButtonController._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && RoleButtonController._GameInfo.IsPlayerAlive)
                {
                    _PlayerActionOnDifferentRoles.PlayerActionInNightPhase(CanPlayerBeActiveInNightPhase, RoleButtonController);
                    _PlayerActionOnDifferentRoles.PlayerActionInDayPhase(CanPlayerBeActiveInDayPhase, RoleButtonController);
                }

                _ProfileForGameScene.OnClickRoleButton(!CanPlayerBeActiveInNightPhase && !CanPlayerBeActiveInDayPhase, RoleButtonController._OwnerInfo.OwnerActorNumber, RoleButtonController._OwnerInfo.OwenrUserId);
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
}
