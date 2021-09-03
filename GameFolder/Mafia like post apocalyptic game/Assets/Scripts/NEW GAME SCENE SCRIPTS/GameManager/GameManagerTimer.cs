﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerTimer : MonoBehaviourPun
{
    [Serializable] public class Timer
    {
        [SerializeField] int seconds;
        [SerializeField] int nightsCount;
        [SerializeField] int daysCount;
        [SerializeField] bool nightTime;
        [SerializeField] bool dayTime;
        [SerializeField] bool hasGameStartVFXInstantiated;
        [SerializeField] Text timerText;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject moon;
       
        public int Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        public int NightsCount
        {
            get => nightsCount;
            set => nightsCount = value;
        }
        public int DaysCount
        {
            get => daysCount;
            set => daysCount = value;
        }
        public bool NightTime
        {
            get => nightTime;
            set => nightTime = value;
        }
        public bool DayTime
        {
            get => dayTime;
            set => dayTime = value;
        }
        public bool HasGameStartVFXInstantiated
        {
            get => hasGameStartVFXInstantiated;
            set => hasGameStartVFXInstantiated = value;
        }
        public string TimerText
        {
            get => timerText.text;
            set => timerText.text = value;
        }
        public GameObject Sun
        {
            get => sun;
        }
        public GameObject Moon
        {
            get => moon;
        }        
    }   

    public Timer _Timer;

    UISoundsInGame _UISoundsInGame { get; set; }
    GameManagerVFXHolder _VfxHolder { get; set; }
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles { get; set; }
    GameManagerPlayerVotesController _GameManagerPlayerVotesController { get; set; }

    const byte CreateVFX = 0;
    const byte EverySecond = 1;

    int roleButtonIconIndex = -1;


    void Awake()
    {
        _VfxHolder = GetComponent<GameManagerVFXHolder>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
        _UISoundsInGame = FindObjectOfType<UISoundsInGame>();
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    void Start()
    {
        _Timer.NightTime = true;
        _Timer.Moon.SetActive(true);
        _Timer.Sun.SetActive(false);
    }

    void Update()
    {
        if (_Timer.Moon.activeInHierarchy != _Timer.NightTime) _Timer.Moon.SetActive(_Timer.NightTime);
        if (_Timer.Sun.activeInHierarchy != _Timer.DayTime) _Timer.Sun.SetActive(_Timer.DayTime);
    }

    #region RunTimer
    internal void RunTimer()
    {
        StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }
    #endregion

    #region TimerCoroutine
    IEnumerator TimerCoroutine(int currentSeconds)
    {
        while (photonView.IsMine)
        {
            _Timer.Seconds = currentSeconds;

            InitializeRaiseEvent(out object[] datas, out RaiseEventOptions _RaiseEventOptions);

            PhotonNetwork.RaiseEvent(CreateVFX, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);

            while (true && photonView.IsMine)
            {
                if (_Timer.NightTime && _Timer.Seconds <= 0)
                {
                    _Timer.Seconds = 90;
                    _Timer.NightsCount++;
                    _Timer.NightTime = false;
                    _Timer.DayTime = true;
                }
                if (_Timer.DayTime && _Timer.Seconds <= 0)
                {
                    _Timer.Seconds = 60;
                    _Timer.DaysCount++;
                    _Timer.DayTime = false;
                    _Timer.NightTime = true;
                }

                _Timer.Seconds--;
                _Timer.TimerText = _Timer.Seconds.ToString("D2");

                PhotonNetwork.RaiseEvent(EverySecond, datas, _RaiseEventOptions, ExitGames.Client.Photon.SendOptions.SendUnreliable);

                yield return new WaitForSeconds(1);
            }
        }
    }
    #endregion

    #region NetworkingClient_EventReceived
    void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code == CreateVFX)
        {
            OnCreateVFX(obj);
        }
        if (obj.Code == EverySecond)
        {
            OnPlaySoundFX(obj);

            PlayerGameControllerCallback(obj, PlayerGameController =>
            {
                CheckPlayerParticipationInVoting(PlayerGameController);
                OnNightPhase(PlayerGameController);
                OnDayPhase(PlayerGameController);
                OnResetPhases(PlayerGameController);
            });

            OnPlayersVotes(obj);
        }
    }
    #endregion

    #region Raise Events

    #region InitializeRaiseEvent
    void InitializeRaiseEvent(out object[] datas, out RaiseEventOptions _RaiseEventOptions)
    {
        datas = new object[]
        {
             "CreateVFX",
             "PlaySoundFX",
             "PlayerActivity",
             "PlayersVotes"
        };

        _RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    }
    #endregion

    #region OnCreateVFX
    void OnCreateVFX(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[0] == "CreateVFX")
        {
            if (!_Timer.HasGameStartVFXInstantiated)
            {
                _VfxHolder.CreateVFX(0);
                _Timer.HasGameStartVFXInstantiated = true;
            }
        }
    }
    #endregion

    #region OnPlaySoundFX
    void OnPlaySoundFX(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[1] == "PlaySoundFX")
        {
            if (_Timer.Seconds <= 10) _UISoundsInGame.PlaySoundFX(0);
        }
    }
    #endregion

    #region PlayerGameControllerCallback
    void PlayerGameControllerCallback(ExitGames.Client.Photon.EventData obj, Action<IPlayerGameController> Callback)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[2] == "PlayerActivity")
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                GameObject playerTagObj = (GameObject)player.TagObject != null ? (GameObject)player.TagObject : null;

                if (playerTagObj != null)
                {
                    IPlayerGameController playerController = playerTagObj.GetComponent<IPlayerGameController>();

                    if (roleButtonIconIndex == -1 && PlayerBaseConditions.PlayerRoleName(playerController.PhotonView.OwnerActorNr) != null) roleButtonIconIndex = PlayerBaseConditions.PlayerRoleName(playerController.PhotonView.OwnerActorNr) == RoleNames.Medic ? 1 : 0;

                    Callback(playerController);
                }
            }
        }
    }
    #endregion

    #region CheckPlayerParticipationInVoting
    void CheckPlayerParticipationInVoting(IPlayerGameController playerController)
    {
        if(Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr) != null)
        {
            if (playerController.IsPlayerAlive != Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr)._GameInfo.IsPlayerAlive)
                playerController.IsPlayerAlive = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr)._GameInfo.IsPlayerAlive;

            if (_Timer.NightTime && _Timer.Seconds <= 30 && playerController.IsPlayerAlive)
                playerController.CanPlayerBeActiveInNightPhase = true;
            else playerController.CanPlayerBeActiveInNightPhase = false;

            if (_Timer.DayTime && _Timer.Seconds <= 60 && playerController.IsPlayerAlive)
                playerController.CanPlayerBeActiveInDayPhase = true;
            else playerController.CanPlayerBeActiveInDayPhase = false;
        }
    }
    #endregion

    #region OnNightPhase
    void OnNightPhase(IPlayerGameController playerController)
    {
        if (IsNightPhase())
        {
            if (playerController.CanPlayerBeActiveInNightPhase)
            {
                if (playerController.PhotonView.IsMine)
                {
                    if (PlayerHasntVotedYet(playerController, 0))
                    {
                        ActivateGameobjectActivityForAllRoleButtons();
                    }
                    if(PlayerHasVoted(playerController, 0))
                    {
                        DeactivateGameobjectActivityForAllRoleButtons();
                    }
                }

                if (DoesVotedConditionsExist(playerController))
                {
                    _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][1] = false;
                }
            }
        }     
    }   
    #endregion

    #region OnDayPhase
    void OnDayPhase(IPlayerGameController playerController)
    {
        if (IsDayPhase())
        {
            if (playerController.CanPlayerBeActiveInDayPhase)
            {
                if (playerController.PhotonView.IsMine)
                {
                    if (PlayerHasntVotedYet(playerController, 1))
                    {
                        ActivateGameobjectActivityForAllRoleButtons();
                    }
                    if (PlayerHasVoted(playerController, 1))
                    {
                        DeactivateGameobjectActivityForAllRoleButtons();
                    }
                }

                if (DoesVotedConditionsExist(playerController))
                {
                    _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][0] = false;
                }
            }
        }
    }
    #endregion

    #region Conditions
    void ActivateGameobjectActivityForAllRoleButtons()
    {
        foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            if (roleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                roleButton.GameObjectActivity(roleButtonIconIndex, true);
            }
        }
    }

    void DeactivateGameobjectActivityForAllRoleButtons()
    {
        foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            roleButton.GameobjectActivityForAllRoleButtons(roleButtonIconIndex, false);
        }
    }

    bool IsNightPhase()
    {
        return _Timer.NightTime && _Timer.Seconds <= 30;
    }

    bool IsDayPhase()
    {
        return _Timer.DayTime && _Timer.Seconds <= 60;
    }

    bool DoesVotedConditionsExist(IPlayerGameController playerController)
    {
        return _GameManagerPlayerVotesController._Votes.PlayerVoteCondition.ContainsKey(playerController.PhotonView.OwnerActorNr);
    }

    bool PlayerHasntVotedYet(IPlayerGameController playerController, int timePhaseIndex)
    {
        return !DoesVotedConditionsExist(playerController) || _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][timePhaseIndex] == false;
    }

    bool PlayerHasVoted(IPlayerGameController playerController, int timePhaseIndex)
    {
        return DoesVotedConditionsExist(playerController) && _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][timePhaseIndex] == true;
    }
    #endregion

    #region OnResetPhases
    void OnResetPhases(IPlayerGameController playerController)
    {
        if (_Timer.NightTime && _Timer.Seconds > 30 || _Timer.DayTime && _Timer.Seconds > 60)
        {
            if (DoesVotedConditionsExist(playerController))
            {
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][0] = false;
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][1] = false;
            }
            playerController.HasVotePhaseResetted = true;

            if (playerController.PhotonView.IsMine)
            {
                DeactivateGameobjectActivityForAllRoleButtons();
            }

            ResetPlayersVotes();
        }
    }
    #endregion

    #region OnPlayersVotes
    void OnPlayersVotes(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[3] == "PlayersVotes")
        {
            foreach (var votes in _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst)
            {
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == votes.Key)._UI.VotesCount = votes.Value;
            }
        }
    }
    #endregion

    #region ResetPlayersVotes
    void ResetPlayersVotes()
    {
        _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst = new System.Collections.Generic.Dictionary<int, int>();

        foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            roleButton._UI.VotesCount = 0;
        }
    }
    #endregion

    #endregion
}

