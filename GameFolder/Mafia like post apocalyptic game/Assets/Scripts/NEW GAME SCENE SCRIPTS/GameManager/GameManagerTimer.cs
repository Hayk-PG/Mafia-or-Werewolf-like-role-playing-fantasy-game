using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

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
    [Serializable] public class LostPlayer
    {
        [SerializeField] bool hasLostPlayerSet;

        public bool HasLostPlayerSet
        {
            get => hasLostPlayerSet;
            set => hasLostPlayerSet = value;
        }

        public Dictionary<int, bool> LostPlayers = new Dictionary<int, bool>();
    }
    [Serializable] public class Teams
    {
        [SerializeField] bool isTeamsCountSet;
        [SerializeField] bool isTeamsCountUpdated;

        public bool IsTeamsCountSet
        {
            get => isTeamsCountSet;
            set => isTeamsCountSet = value;
        }
        public bool IsTeamsCountUpdated
        {
            get => isTeamsCountUpdated;
            set => isTeamsCountUpdated = value;
        }
    }
    struct PhasesIcons
    {
        [SerializeField] bool isNightPhaseIconsActive;
        [SerializeField] bool isDayPhaseIconsActive;

        internal bool IsNightPhaseIconsActive
        {
            get => isNightPhaseIconsActive;
            set => isNightPhaseIconsActive = value;
        }
        internal bool IsDayPhaseIconsActive
        {
            get => isDayPhaseIconsActive;
            set => isDayPhaseIconsActive = value;
        }
    }

    public Timer _Timer;
    public LostPlayer _LostPlayer;
    public Teams _Teams;
    PhasesIcons _PhasesIcons;

    TimerTickSound _TimerTickSound { get; set; }
    GameManagerVFXHolder _VfxHolder { get; set; }
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles { get; set; }
    GameManagerPlayerVotesController _GameManagerPlayerVotesController { get; set; }
    TeamsController _TeamsController { get; set; }

    public delegate void PhaseCallback(bool isResetPhase);
    public PhaseCallback IsResetPhaseActive;


    void Awake()
    {
        _VfxHolder = GetComponent<GameManagerVFXHolder>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
        _TeamsController = GetComponent<TeamsController>();
        _TimerTickSound = FindObjectOfType<TimerTickSound>();
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

        _LostPlayer.LostPlayers = FindObjectOfType<GameManagerTimer>()._LostPlayer.LostPlayers;
        _Teams.IsTeamsCountUpdated = false;
    }
    #endregion

    #region TimerCoroutine
    IEnumerator TimerCoroutine(int currentSeconds)
    {
        while (photonView.IsMine)
        {
            _Timer.Seconds = currentSeconds;

            InitializeRaiseEvent(out object[] datas, out RaiseEventOptions _RaiseEventOptions);

            PhotonNetwork.RaiseEvent(RaiseEventsStrings.GameStartKey, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);

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

                PhotonNetwork.RaiseEvent(RaiseEventsStrings.OnEverySecondKey, datas, _RaiseEventOptions, ExitGames.Client.Photon.SendOptions.SendUnreliable);

                yield return new WaitForSeconds(1);
            }
        }
    }
    #endregion

    #region NetworkingClient_EventReceived
    void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code == RaiseEventsStrings.GameStartKey)
        {
            GameStartVFX(obj);
        }
        if (obj.Code == RaiseEventsStrings.OnEverySecondKey)
        {
            _TimerTickSound.PlayTimerTickingSoundFX(obj, _Timer);

            PlayerGameControllerCallback(obj, PlayerGameController =>
            {
                CheckPlayerParticipationInVoting(PlayerGameController);
                OnNightPhase(PlayerGameController);
                OnDayPhase(PlayerGameController);
                OnResetPhases(PlayerGameController);
                ShareLostPlayers(PlayerGameController);
            });

            OnPlayersVotes(obj);
        }
    }
    #endregion

    #region InitializeRaiseEvent
    void InitializeRaiseEvent(out object[] datas, out RaiseEventOptions _RaiseEventOptions)
    {
        datas = new object[]
        {
             RaiseEventsStrings.CreateGameStartVFX,
             RaiseEventsStrings.PlayTimerTickingSoundFX,
             RaiseEventsStrings.PlayerActivity,
             RaiseEventsStrings.PlayersVotes
        };

        _RaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    }
    #endregion

    #region GameStartVFX
    void GameStartVFX(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[0] == RaiseEventsStrings.CreateGameStartVFX)
        {
            if (!_Timer.HasGameStartVFXInstantiated)
            {
                _VfxHolder.CreateVFX(0);
                _Timer.HasGameStartVFXInstantiated = true;
            }
        }
    }
    #endregion

    #region PlayerGameControllerCallback
    void PlayerGameControllerCallback(ExitGames.Client.Photon.EventData obj, Action<IPlayerGameController> Callback)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[2] == RaiseEventsStrings.PlayerActivity)
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                GameObject playerTagObj = (GameObject)player.TagObject != null ? (GameObject)player.TagObject : null;

                if (playerTagObj != null)
                {
                    IPlayerGameController playerController = playerTagObj.GetComponent<IPlayerGameController>();
             
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
            _LostPlayer.HasLostPlayerSet = false;
            _Teams.IsTeamsCountUpdated = false;

            if (playerController.CanPlayerBeActiveInNightPhase)
            {
                if (playerController.PhotonView.IsMine)
                {
                    if (!_PhasesIcons.IsNightPhaseIconsActive)
                    {
                        if (PlayerHasntVotedYet(playerController, 0) && PlayerBaseConditions.PlayerRoleName(playerController.PhotonView.OwnerActorNr) != RoleNames.Citizen)
                        {
                            ActivateGameobjectActivityForAllRoleButtons();
                        }
                        if (PlayerHasVoted(playerController, 0))
                        {
                            DeactivateGameobjectActivityForAllRoleButtons();
                        }

                        _PhasesIcons.IsNightPhaseIconsActive = true;
                        _PhasesIcons.IsDayPhaseIconsActive = false;
                    }

                    RoleButtonsVoteCountTextVisibility(playerController, true, false, false);

                    SheriffDiscoverTheRole(playerController);
                }

                if (DoesVotedConditionsExist(playerController))
                {
                    _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][1] = false;
                }
            }

            IsResetPhaseActive?.Invoke(false);
            MedicHealsThePlayer();
            InfectedsVotes(playerController);           
        }

        InfectedTeam(playerController);
    }   
    #endregion

    #region OnDayPhase
    void OnDayPhase(IPlayerGameController playerController)
    {
        if (IsDayPhase())
        {
            _LostPlayer.HasLostPlayerSet = false;
            _Teams.IsTeamsCountUpdated = false;

            if (playerController.CanPlayerBeActiveInDayPhase)
            {
                if (playerController.PhotonView.IsMine)
                {
                    if (!_PhasesIcons.IsDayPhaseIconsActive)
                    {
                        if (PlayerHasntVotedYet(playerController, 1))
                        {
                            ActivateGameobjectActivityForAllRoleButtons();
                        }
                        if (PlayerHasVoted(playerController, 1))
                        {
                            DeactivateGameobjectActivityForAllRoleButtons();
                        }

                        _PhasesIcons.IsDayPhaseIconsActive = true;
                        _PhasesIcons.IsNightPhaseIconsActive = false;
                    }

                    RoleButtonsVoteCountTextVisibility(playerController, false, true, false);
                }

                if (DoesVotedConditionsExist(playerController))
                {
                    _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][0] = false;
                }
            }

            IsResetPhaseActive?.Invoke(false);
        }
    }
    #endregion

    #region Conditions

    void LoopRoleButtonsCallback(Action<RoleButtonController> RoleButton)
    {
        foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            RoleButton?.Invoke(roleButton);
        }
    }

    void ActivateGameobjectActivityForAllRoleButtons()
    {
        LoopRoleButtonsCallback(RoleButton => 
        {
            if (RoleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                if (RoleButton._GameInfo.IsPlayerAlive) RoleButton.VoteFXActivity(true, false);
            }
        });
    }

    void RoleButtonsVoteCountTextVisibility(IPlayerGameController playerController, bool isNightPhase, bool isDayPhase, bool resetPhase)
    {
        if (isNightPhase)
        {
            LoopRoleButtonsCallback(RoleButton =>
            {
                if (PlayerBaseConditions.PlayerRoleName(playerController.PhotonView.OwnerActorNr) == RoleNames.Infected)
                {
                    if (RoleButton._GameInfo.IsPlayerAlive) { MyCanvasGroups.CanvasGroupActivity(RoleButton._UI.VotesCountTextCanvasGroup, true); }
                }
                else
                {
                    MyCanvasGroups.CanvasGroupActivity(RoleButton._UI.VotesCountTextCanvasGroup, false);
                }
            });
        }
        if (isDayPhase)
        {
            LoopRoleButtonsCallback(RoleButton =>
            {
                if (RoleButton._GameInfo.IsPlayerAlive) MyCanvasGroups.CanvasGroupActivity(RoleButton._UI.VotesCountTextCanvasGroup, true);
            });
        }
        if (resetPhase)
        {
            LoopRoleButtonsCallback(RoleButton =>
            {
                MyCanvasGroups.CanvasGroupActivity(RoleButton._UI.VotesCountTextCanvasGroup, false);
            });
        }
    }

    void DeactivateGameobjectActivityForAllRoleButtons()
    {
        LoopRoleButtonsCallback(RoleButton =>
        {
            if (RoleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                RoleButton.VoteFXActivity(false, false);
            }
        });
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
            GetTeamsCount();

            if (DoesVotedConditionsExist(playerController))
            {
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][0] = false;
                _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[playerController.PhotonView.OwnerActorNr][1] = false;
                _PhasesIcons.IsDayPhaseIconsActive = false;
                _PhasesIcons.IsNightPhaseIconsActive = false;
            }
            playerController.HasVotePhaseResetted = true;

            if (playerController.PhotonView.IsMine)
            {
                DeactivateGameobjectActivityForAllRoleButtons();
                RoleButtonsVoteCountTextVisibility(playerController, false, false, true);
            }

            IsResetPhaseActive?.Invoke(true);
            GetLostPlayer();
            OnResetPlayersVotes();
        }

        ResetLizardsVotes();
    }
    #endregion

    #region ResetLizardsVotes
    void ResetLizardsVotes()
    {
        if (_Timer.NightTime && _Timer.Seconds > 30)
        {
            if (_GameManagerPlayerVotesController._Votes.LizardVoteAgainst.Count > 0) _GameManagerPlayerVotesController._Votes.LizardVoteAgainst = new Dictionary<int, bool>();
        }
    }
    #endregion

    #region MedicHealsThePlayer
    void MedicHealsThePlayer()
    {
        foreach (var heal in _GameManagerPlayerVotesController._Votes.HealedPlayers)
        {
            Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == heal.Key)._GameInfo.IsPlayerHealed = heal.Value;
        }
    }
    #endregion

    #region SheriffDiscoverTheRole
    void SheriffDiscoverTheRole(IPlayerGameController playerController)
    {
        foreach (var role in _GameManagerPlayerVotesController._Votes.DiscoverTheRole)
        {
            Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == role.Key)._UI.VisibleToEveryoneImage = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == role.Key)._UI.RoleImage;
        }
    }
    #endregion

    #region InfectedsVotes
    void InfectedsVotes(IPlayerGameController playerController)
    {
        foreach (var victim in _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst)
        {
            Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == victim.Key)._UI.VotesCount = victim.Value;
        }
    }
    #endregion

    #region InfectedTeam
    void InfectedTeam(IPlayerGameController playerController)
    {
        if (playerController.PhotonView.IsMine)
        {
            if (PlayerBaseConditions.PlayerRoleName(playerController.PhotonView.OwnerActorNr) == RoleNames.Infected)
            {
                LoopRoleButtonsCallback(RoleButton =>
                {
                    if (RoleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && RoleButton._GameInfo.RoleName == RoleNames.Infected)
                    {
                        if (RoleButton._UI.VisibleToEveryoneImage != RoleButton._UI.RoleImage) RoleButton._UI.VisibleToEveryoneImage = RoleButton._UI.RoleImage;
                    }
                });
            }
        }
    }
    #endregion

    #region SetLostPlayer
    void GetLostPlayer()
    {
        if (!_LostPlayer.HasLostPlayerSet && photonView.IsMine)
        {
            Dictionary<int, int> _playersReceivedVotesCount = new Dictionary<int, int>();

            List<int> playersReceivedVotesCount = new List<int>();

            LoopRoleButtonsCallback(Rolebutton =>
            {
                if (Rolebutton._UI.VotesCount > 0 && !Rolebutton._GameInfo.IsPlayerHealed) playersReceivedVotesCount.Add(Rolebutton._UI.VotesCount);
            });

            playersReceivedVotesCount.Sort();

            SetLostPlayer(playersReceivedVotesCount, LostPlayer =>
            {
                if (!_LostPlayer.LostPlayers.ContainsKey(LostPlayer._OwnerInfo.OwnerActorNumber))
                {
                    _LostPlayer.LostPlayers.Add(LostPlayer._OwnerInfo.OwnerActorNumber, false);
                }
                else
                {
                    _LostPlayer.LostPlayers[LostPlayer._OwnerInfo.OwnerActorNumber] = false;
                }                   
            });

            SoldierVote(Key => 
            {
                if (!_LostPlayer.LostPlayers.ContainsKey(Key))
                {
                    _LostPlayer.LostPlayers.Add(Key, false);
                }
                else
                {
                    _LostPlayer.LostPlayers[Key] = false;
                }
            });           
        }      
    }

    void SetLostPlayer(List<int> playersReceivedVotesCount, Action<RoleButtonController> Callback)
    {
        if (playersReceivedVotesCount.Count > 0)
        {
            foreach (var roleButton in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
            {
                if (roleButton._UI.VotesCount >= playersReceivedVotesCount[playersReceivedVotesCount.Count - 1])
                {
                    Callback(roleButton);

                    _LostPlayer.HasLostPlayerSet = true;

                    break;
                }
            }
        }
    }

    void SoldierVote(Action<int> Key)
    {
        foreach (var soldierVote in _GameManagerPlayerVotesController._Votes.SoldierVoteAgainst)
        {
            Key?.Invoke(soldierVote.Key);
        }
    }

    void ShareLostPlayers(IPlayerGameController playerController)
    {
        if (playerController.PhotonView.IsMine)
        {
            foreach (var lostPlayer in _LostPlayer.LostPlayers)
            {
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == lostPlayer.Key)._GameInfo.IsPlayerAlive = lostPlayer.Value;
            }
        }      
    }
    #endregion

    #region OnPlayersVotes
    void OnPlayersVotes(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[3] == RaiseEventsStrings.PlayersVotes)
        {          
            foreach (var votes in _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst)
            {
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == votes.Key)._UI.VotesCount = votes.Value;
            }

            foreach (var votes in _GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted)
            {
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, _roleButton => _roleButton._OwnerInfo.OwnerActorNumber == votes.Key).VotedNameIconActivity(true, votes.Value);              
            }
        }
    }
    #endregion

    #region OnResetPlayersVotes
    void OnResetPlayersVotes()
    {
        LoopRoleButtonsCallback(RoleButton => 
        {
            int key = RoleButton._OwnerInfo.OwnerActorNumber;

            if (_GameManagerPlayerVotesController._Votes.PlayersVotesAgainst.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst[key] = 0;
            if (_GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted[key] = "";
            if (_GameManagerPlayerVotesController._Votes.HealedPlayers.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.HealedPlayers[key] = false;
            if (_GameManagerPlayerVotesController._Votes.DiscoverTheRole.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.DiscoverTheRole[key] = false;
            if (_GameManagerPlayerVotesController._Votes.InfectedVotesAgainst.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst[key] = 0;

            RoleButton._UI.VotesCount = 0;
            RoleButton._GameInfo.IsPlayerHealed = false;
            RoleButton.VotedNameIconActivity(false, null);
        });


        //if (_GameManagerPlayerVotesController._Votes.PlayersVotesAgainst.Count > 0) _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst = new Dictionary<int, int>();
        //if (_GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted.Count > 0) _GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted = new Dictionary<int, string>();
        //if (_GameManagerPlayerVotesController._Votes.HealedPlayers.Count > 0) _GameManagerPlayerVotesController._Votes.HealedPlayers = new Dictionary<int, bool>();
        //if (_GameManagerPlayerVotesController._Votes.DiscoverTheRole.Count > 0) _GameManagerPlayerVotesController._Votes.DiscoverTheRole = new Dictionary<int, bool>();
        //if (_GameManagerPlayerVotesController._Votes.InfectedVotesAgainst.Count > 0) _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst = new Dictionary<int, int>();

        OnUpdateTeamsCount();
    }
    #endregion

    #region GetTeamsCount
    void GetTeamsCount()
    {
        if (photonView.IsMine && !_Teams.IsTeamsCountSet && _GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet)
        {
            _TeamsController.GetTeamsCount(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons);
            _Teams.IsTeamsCountSet = true;
        }
    }
    #endregion

    #region OnUpdateTeamsCount
    void OnUpdateTeamsCount()
    {
        if (photonView.IsMine)
        {
            //if (!_Teams.IsTeamsCountUpdated)
            //{
            //    _TeamsController.UpdateTeamsCount();
            //    _Teams.IsTeamsCountUpdated = true;
            //    print("Teams count updated");
            //}

            _TeamsController.UpdateTeamsCount();
        }
    }
    #endregion
}

