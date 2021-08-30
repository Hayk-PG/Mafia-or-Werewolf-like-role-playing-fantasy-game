using Photon.Pun;
using System;
using UnityEngine;

public class PlayerRaiseEventListener : MonoBehaviourPun
{
    [Serializable] class Conditions
    {
        [SerializeField] internal bool NightVoteEventReceived;
        [SerializeField] internal bool DayVoteEventReceived;
        [SerializeField] internal bool VotesConditionsBeenResetted;
    }

    [SerializeField] Conditions _Conditions;

    GameManagerTimer _GameManagerTimer;


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        OnNightVote(obj);
        OnDayVote(obj);
        OnResetVotesConditions(obj);
    }

    void OnNightVote(ExitGames.Client.Photon.EventData obj)
    {
        if (_GameManagerTimer._Timer.NightTime && _GameManagerTimer._Timer.Seconds <= 30 && !_Conditions.NightVoteEventReceived)
        {
            if (obj.Code == 1)
            {
                object[] datas = (object[])obj.CustomData;

                if ((string)datas[2] == "NightVote")
                {
                    if (photonView.IsMine)
                    {
                        foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                        {
                            if(roleButton.GetComponent<IRoleButtonGamePhaseController>().RoleButtonController._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                            {
                                roleButton?.GetComponent<IRoleButtonGamePhaseController>().ActivateAimObj(true);
                            }
                        }

                        _Conditions.NightVoteEventReceived = true;
                        _Conditions.DayVoteEventReceived = false;
                        _Conditions.VotesConditionsBeenResetted = false;
                    }
                }               
            }
        }
    }

    void OnDayVote(ExitGames.Client.Photon.EventData obj)
    {

        if (_GameManagerTimer._Timer.DayTime && _GameManagerTimer._Timer.Seconds <= 60 && !_Conditions.DayVoteEventReceived)
        {
            if (obj.Code == 1)
            {
                object[] datas = (object[])obj.CustomData;

                if ((string)datas[3] == "DayVote")
                {
                    if (photonView.IsMine)
                    {
                        foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                        {
                            if (roleButton.GetComponent<IRoleButtonGamePhaseController>().RoleButtonController._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                            {
                                roleButton?.GetComponent<IRoleButtonGamePhaseController>().ActivateAimObj(true);
                            }
                        }

                        _Conditions.DayVoteEventReceived = true;
                        _Conditions.NightVoteEventReceived = false;
                        _Conditions.VotesConditionsBeenResetted = false;
                    }
                }
            }
        }
    }

    void OnResetVotesConditions(ExitGames.Client.Photon.EventData obj)
    {
        if (!_Conditions.VotesConditionsBeenResetted)
        {
            if (_GameManagerTimer._Timer.DayTime && _GameManagerTimer._Timer.Seconds > 60 || _GameManagerTimer._Timer.NightTime && _GameManagerTimer._Timer.Seconds > 30)
            {
                if (obj.Code == 1)
                {
                    object[] datas = (object[])obj.CustomData;

                    if (photonView.IsMine)
                    {
                        if ((string)datas[4] == "ResetVotesConditions")
                        {
                            foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                            {
                                roleButton?.GetComponent<IRoleButtonGamePhaseController>().ActivateAimObj(false);
                            }

                            _Conditions.VotesConditionsBeenResetted = true;
                            _Conditions.DayVoteEventReceived = false;
                            _Conditions.NightVoteEventReceived = false;
                        }
                    }
                }
            }
        }
    }
}
