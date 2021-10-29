using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class GameManagerTimer : MonoBehaviourPun,IReset
{
    [Serializable] public class Timer
    {
        [SerializeField] int seconds;
        [SerializeField] int gameEndSeconds;
        [SerializeField] int nightsCount;
        [SerializeField] int daysCount;
        [SerializeField] float nextPhaseWaitUntil;
        [SerializeField] bool wasNextWaitUntilRunning;
        [SerializeField] bool nightTime;
        [SerializeField] bool dayTime;
        [SerializeField] bool hasGameStartVFXInstantiated;
        [SerializeField] bool isGameFinished;
        [SerializeField] Text timerText;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject moon;
       
        public int Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        public int GameEndSeconds
        {
            get => gameEndSeconds;
            set => gameEndSeconds = value;
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
        public float NextPhaseWaitUntil
        {
            get => nextPhaseWaitUntil;
            set => nextPhaseWaitUntil = value;
        }
        public bool WasNextWaitUntilRunning
        {
            get => wasNextWaitUntilRunning;
            set => wasNextWaitUntilRunning = value;
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
        public bool IsGameFinished
        {
            get => isGameFinished;
            set => isGameFinished = value;
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
        public IEnumerator Coroutine { get; set; }
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
    [Serializable] public class GameEndData
    {
        public bool HumansWin;
        public Dictionary<string, string> PlayersCachedNames { get; set; }
        public Dictionary<string, string> PlayersRolesInLastRound { get; set; }
        public Dictionary<string, string> PlayersProfilePictureURL { get; set; }
        public Dictionary<string, int> PointsOfTheDoctor { get; set; }
        public Dictionary<string, int> PointsOfTheSheriff { get; set; }
        public Dictionary<string, int> PointsOfTheSoldier { get; set; }
        public Dictionary<string, int> PointsOfTheInfected { get; set; }
        public Dictionary<string, int> PointsOfTheLizard { get; set; }
        public Dictionary<string, int> PointsForEveryone { get; set; }
    }
    public struct PhasesIcons
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
    public GameEndData _GameEndData;
    public PhasesIcons _PhasesIcons;

    TimerTickSound _TimerTickSound { get; set; }
    GameManagerVFXHolder _VfxHolder { get; set; }
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles { get; set; }
    GameManagerPlayerVotesController _GameManagerPlayerVotesController { get; set; }
    TeamsController _TeamsController { get; set; }
    CardsTabController _CardsTabController { get; set; }
    EndTab _EndTab { get; set; }
    GameStartAnnouncement _GameStartAnnouncement { get; set; }

    public delegate void PhaseCallback(bool isResetPhase);
    public PhaseCallback IsResetPhaseActive;

    [SerializeField] bool test;


    void Awake()
    {
        _VfxHolder = GetComponent<GameManagerVFXHolder>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
        _TeamsController = GetComponent<TeamsController>();
        _TimerTickSound = FindObjectOfType<TimerTickSound>();
        _CardsTabController = FindObjectOfType<CardsTabController>();
        _EndTab = FindObjectOfType<EndTab>();
        _GameStartAnnouncement = GetComponent<GameStartAnnouncement>();
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

        InitializeGameEndDataDictionaries();
    }

    void Update()
    {
        if (_Timer.Moon.activeInHierarchy != _Timer.NightTime) _Timer.Moon.SetActive(_Timer.NightTime);
        if (_Timer.Sun.activeInHierarchy != _Timer.DayTime) _Timer.Sun.SetActive(_Timer.DayTime);

        RunNextPhaseUntilTimer(/*delegate { _CardsTabController.OnDeathTab(true, null); }*/ null);
        StopRunNextPhaseUntilTimer(delegate{ _CardsTabController.OnDeathTab(false, null); });
    }

    #region RunTimer
    internal void RunTimer()
    {
        _Timer.Coroutine = null;
        _Timer.Coroutine = TimerCoroutine(_Timer.Seconds, _Timer.IsGameFinished);

        if (!_Timer.IsGameFinished)
        {
            StartCoroutine(_Timer.Coroutine);
            _EndTab?.GetComponent<IReset>().ResetAtTheEndOfTheGameEndCoroutine();
        }
       
        _LostPlayer.LostPlayers = FindObjectOfType<GameManagerTimer>()._LostPlayer.LostPlayers;
        _Teams.IsTeamsCountUpdated = false;
    } 
    
    internal void RunGameEndTimer()
    {
        if (_Timer.IsGameFinished)
        {
            StartCoroutine(GameEndTimerCoroutine(_Timer.GameEndSeconds, _Timer.IsGameFinished));
        }
    }
    #endregion

    #region TimerCoroutine
    IEnumerator TimerCoroutine(int currentSeconds, bool isGameFinished)
    {
        while (photonView.IsMine)
        {
            _Timer.Seconds = currentSeconds;

            InitializeRaiseEvent(out object[] datas, out RaiseEventOptions _RaiseEventOptions);

            PhotonNetwork.RaiseEvent(RaiseEventsStrings.GameStartKey, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);

            while (!isGameFinished && photonView.IsMine)
            {
                if (_Timer.NextPhaseWaitUntil <= 0)
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
                    _Timer.WasNextWaitUntilRunning = false;                   
                }

                PhotonNetwork.RaiseEvent(RaiseEventsStrings.OnEverySecondKey, datas, _RaiseEventOptions, ExitGames.Client.Photon.SendOptions.SendUnreliable);

                yield return new WaitForSeconds(1);
            }
        }
    }
    #endregion

    #region RunNextPhaseUntilTimer
    void RunNextPhaseUntilTimer(Action DoSomething)
    {
        if(_Timer.NightsCount > 0)
        {
            if(_Timer.NightTime && _Timer.Seconds >= 59 || _Timer.DayTime && _Timer.DayTime && _Timer.Seconds >= 89)
            {
                if (_Timer.NextPhaseWaitUntil < 5 && !_Timer.WasNextWaitUntilRunning)
                {
                    _Timer.NextPhaseWaitUntil += Time.deltaTime;
                    DoSomething?.Invoke();                   
                }
            }
        }
    }
    #endregion

    #region StopRunNextPhaseUntilTimer
    void StopRunNextPhaseUntilTimer(Action DoSomething)
    {
        if (_Timer.NextPhaseWaitUntil >= 5 || _Timer.NightTime && _Timer.Seconds <= 55 || _Timer.DayTime && _Timer.Seconds <= 85)
        {
            if (!_Timer.WasNextWaitUntilRunning || _Timer.NextPhaseWaitUntil != 0 || _CardsTabController.CardsTabCanvasGroup.interactable)
            {
                _Timer.WasNextWaitUntilRunning = true;
                _Timer.NextPhaseWaitUntil = 0;
                DoSomething?.Invoke();
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

            CachePlayersData();
            OnPlayersVotes(obj);
            DestroyAwardedCards();
        }
        if (obj.Code == RaiseEventsStrings.GameEndKey)
        {
            OnGameEnd();
        }
        if (obj.Code == RaiseEventsStrings.StartNewRoundKey)
        {
            OnGameRestart(obj);
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
                if (playerController.PhotonView.IsMine && playerController.PhotonView.AmOwner)
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
                if (playerController.PhotonView.IsMine && playerController.PhotonView.AmOwner)
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
            if (RoleButton._UI.RoleButtonCanvasGroup.interactable)
            {
                if (RoleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    if (RoleButton._GameInfo.IsPlayerAlive) RoleButton.VoteFXActivity(true, false);
                }
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
                RoleButton._UI.Selected.SetActive(false);
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

            if (playerController.PhotonView.IsMine && playerController.PhotonView.AmOwner)
            {
                DeactivateGameobjectActivityForAllRoleButtons();
                RoleButtonsVoteCountTextVisibility(playerController, false, false, true);
            }

            IsResetPhaseActive?.Invoke(true);
            GetLostPlayer();
            OnResetPlayersVotes();
            GameEnd();
            if (test)
            {
                GameEnd();
                print("test");
                test = false;
            }
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
        foreach (var healedPlayer in _GameManagerPlayerVotesController._Votes.HealedPlayers)
        {
            PlayerBaseConditions.GetRoleButton(healedPlayer.Key)._GameInfo.IsPlayerHealed = healedPlayer.Value;
        }
    }
    #endregion
    
    #region InfectedsVotes
    void InfectedsVotes(IPlayerGameController playerController)
    {
        foreach (var victimByInfecteds in _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst)
        {
            PlayerBaseConditions.GetRoleButton(victimByInfecteds.Key)._UI.VotesCount = victimByInfecteds.Value;
        }
    }
    #endregion

    #region SoldierVote
    void SoldierVote(Action<int> Key)
    {
        foreach (var soldierVote in _GameManagerPlayerVotesController._Votes.SoldierVoteAgainst)
        {
            Key?.Invoke(soldierVote.Key);
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
                if (Rolebutton._UI.VotesCount > 0) playersReceivedVotesCount.Add(Rolebutton._UI.VotesCount);
            });

            playersReceivedVotesCount.Sort();

            SetLostPlayer(playersReceivedVotesCount, LostPlayer =>
            {
                AssignLostPlayer(LostPlayer);
            });

            SoldierVote(Key => 
            {
                AssignLostPlayerBySoldierVote(Key);
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
   
    void ShareLostPlayers(IPlayerGameController playerController)
    {
        if (playerController.PhotonView.IsMine)
        {
            foreach (var lostPlayer in _LostPlayer.LostPlayers)
            {
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == lostPlayer.Key)._GameInfo.IsPlayerAlive = lostPlayer.Value;

                RewardThePlayerForTheRightVote(lostPlayer.Key);
            }
        }      
    }

    #region AssignLostPlayer
    void AssignLostPlayer(RoleButtonController LostPlayer)
    {
        if(LostPlayer._GameInfo.IsPlayerAlive)
        {
            if (!LostPlayer._GameInfo.IsPlayerHealed)
            {
                if (!_LostPlayer.LostPlayers.ContainsKey(LostPlayer._OwnerInfo.OwnerActorNumber))
                {
                    _LostPlayer.LostPlayers.Add(LostPlayer._OwnerInfo.OwnerActorNumber, false);
                }
                else
                {
                    _LostPlayer.LostPlayers[LostPlayer._OwnerInfo.OwnerActorNumber] = false;
                }                
            }
            else
            {
                if (_LostPlayer.LostPlayers.ContainsKey(LostPlayer._OwnerInfo.OwnerActorNumber))
                {
                    RewardTheMedic();

                    _LostPlayer.LostPlayers.Remove(LostPlayer._OwnerInfo.OwnerActorNumber);
                }
                
            }
        }
    }
    #endregion

    #region AssignLostPlayerBySoldierVote
    void AssignLostPlayerBySoldierVote(int Key)
    {
        RoleButtonController LostPlayer = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, _lostPlayer => _lostPlayer._OwnerInfo.OwnerActorNumber == Key);

        if (LostPlayer._GameInfo.IsPlayerAlive)
        {
            if (!LostPlayer._GameInfo.IsPlayerHealed)
            {
                if (!_LostPlayer.LostPlayers.ContainsKey(Key))
                {
                    _LostPlayer.LostPlayers.Add(Key, false);
                }
                else
                {
                    _LostPlayer.LostPlayers[Key] = false;
                }
            }
            else
            {
                if (_LostPlayer.LostPlayers.ContainsKey(Key))
                    _LostPlayer.LostPlayers.Remove(Key);
            }
        }
    }
    #endregion

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
                bool activateVoteNameIcon = _GameManagerPlayerVotesController._Votes.PlayerVoteCondition.ContainsKey(votes.Key) && _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[votes.Key][0] == true || _GameManagerPlayerVotesController._Votes.PlayerVoteCondition.ContainsKey(votes.Key) && _GameManagerPlayerVotesController._Votes.PlayerVoteCondition[votes.Key][1] == true ? true : false;
                Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, _roleButton => _roleButton._OwnerInfo.OwnerActorNumber == votes.Key).VotedNameIconActivity(activateVoteNameIcon && _Timer.DayTime, votes.Value[votes.Value.Length - 1]);              
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
            if (_GameManagerPlayerVotesController._Votes.HealedPlayers.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.HealedPlayers[key] = false;
            if (_GameManagerPlayerVotesController._Votes.DiscoverTheRole.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.DiscoverTheRole[key] = false;
            if (_GameManagerPlayerVotesController._Votes.InfectedVotesAgainst.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst[key] = 0;
            if (_GameManagerPlayerVotesController._Votes.SoldierVoteAgainst.ContainsKey(key)) _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst[key] = 0;

            RoleButton._UI.VotesCount = 0;
            RoleButton._GameInfo.IsPlayerHealed = false;
            RoleButton.VotedNameIconActivity(false, null);
        });

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
            _TeamsController.UpdateTeamsCount();
        }
    }
    #endregion

    #region GameEnd
    void GameEnd()
    {
        if (_Timer.NightsCount > 0 && !_CardsTabController.CardsTabCanvasGroup.interactable)
        {
            if (_TeamsController._TeamsCount.FirstTeamCount > _TeamsController._TeamsCount.SecondTeamCount + 3 || _TeamsController._TeamsCount.SecondTeamCount < 1)
            {
                GameEndControllerByMasterClient(true);               
            }
            if (_TeamsController._TeamsCount.FirstTeamCount < _TeamsController._TeamsCount.SecondTeamCount - 2 || _TeamsController._TeamsCount.FirstTeamCount < 1)
            {
                GameEndControllerByMasterClient(false);
            }
        }
    }
    #endregion

    #region GameEndControllerByMasterClient
    void GameEndControllerByMasterClient(bool humansWin)
    {
        if (photonView.IsMine)
        {
            _GameEndData.HumansWin = humansWin;

            StopCoroutine(_Timer.Coroutine);

            if (!_Timer.IsGameFinished)
            {
                _Timer.IsGameFinished = true;
                StartCoroutine(GameEndTimerCoroutine(_Timer.GameEndSeconds, _Timer.IsGameFinished));
            }    
        }
    }
    #endregion

    #region GameEndTimerCoroutine
    IEnumerator GameEndTimerCoroutine(int currentSeconds, bool isGameFinished)
    {
        while (isGameFinished && photonView.IsMine)
        {
            _Timer.GameEndSeconds = currentSeconds;
            object[] datas = new object[] { RaiseEventsStrings.GameEnd };

            while (isGameFinished && _Timer.GameEndSeconds < 60 && photonView.IsMine)
            {
                _Timer.GameEndSeconds++;               
                yield return new WaitForSeconds(1);
                PhotonNetwork.RaiseEvent(RaiseEventsStrings.GameEndKey, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendUnreliable);
            }

            yield return null;

            if (_Timer.GameEndSeconds >= 60)
            {
                isGameFinished = false;
                object[] datas1 = new object[] { isGameFinished, 0 };
                PhotonNetwork.RaiseEvent(RaiseEventsStrings.StartNewRoundKey, datas1, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);
            }

            yield return null;
        }
    }
    #endregion

    #region OnGameEnd
    void OnGameEnd()
    {
        if (!_EndTab._UI.CanvasGroup.interactable)
        {
            _EndTab.OpenEndTab();

            if (photonView.IsMine)
            {
                UpdatePlayersStatsByMasterClient();
            }

            foreach (var url in _GameEndData.PlayersProfilePictureURL)
            {
                PlayerBaseConditions._LocalPlayerTagObject.GetComponent<PlayerScreenOnGameEnd>().Screen(url.Key, url.Value, _GameEndData.HumansWin);
            }
        }

        if (photonView.IsMine)
        {
            foreach (var iReset in GetComponents<IReset>())
            {
                iReset?.ResetWhileGameEndCoroutineIsRunning();
            }
        }

        foreach (var roleButtons in _GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons)
        {
            roleButtons?.ResetWhileGameEndCoroutineIsRunning();
        }

        foreach (var iReset in PlayerBaseConditions._LocalPlayerTagObject.GetComponents<IReset>())
        {
            iReset?.ResetWhileGameEndCoroutineIsRunning();
        }
    }
    #endregion

    #region UpdatePlayersStatsByMasterClient
    void UpdatePlayersStatsByMasterClient()
    {
        foreach (var data in _GameEndData.PlayersRolesInLastRound)
        {
            int win = 0;
            int lost = 0;

            PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(data.Key,
                GetPlayerStats =>
                {
                    if (_GameEndData.HumansWin)
                    {
                        win = data.Value != RoleNames.Infected && data.Value != RoleNames.Lizard ? 1 : 0;
                        lost = data.Value == RoleNames.Infected || data.Value == RoleNames.Lizard ? 1 : 0;
                    }
                    else
                    {
                        win = data.Value == RoleNames.Infected || data.Value == RoleNames.Lizard ? 1 : 0;
                        lost = data.Value != RoleNames.Infected && data.Value != RoleNames.Lizard ? 1 : 0;
                    }

                    PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(data.Key, UpdatePlayerStats =>
                    {
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Rank, Value = GetPlayerStats.Rank < 1 ? 1 : GetPlayerStats.Rank });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.TotalTimePlayed, Value = GetPlayerStats.TotalTimePlayed += 1 });

                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Win, Value = GetPlayerStats.Win += win });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Lost, Value = GetPlayerStats.Lost += lost });

                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSurvivor, Value = GetPlayerStats.RolesPlayedCount[0] += data.Value == RoleNames.Citizen ? 1 : 0 });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsDoctor, Value = GetPlayerStats.RolesPlayedCount[1] += data.Value == RoleNames.Medic ? 1 : 0 });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSheriff, Value = GetPlayerStats.RolesPlayedCount[2] += data.Value == RoleNames.Sheriff ? 1 : 0 });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSoldier, Value = GetPlayerStats.RolesPlayedCount[3] += data.Value == RoleNames.Soldier ? 1 : 0 });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsInfected, Value = GetPlayerStats.RolesPlayedCount[4] += data.Value == RoleNames.Infected ? 1 : 0 });
                        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsWitch, Value = GetPlayerStats.RolesPlayedCount[5] += data.Value == RoleNames.Lizard ? 1 : 0 });
                    });
                });
        }
    }
    #endregion

    #region OnGameRestart
    void OnGameRestart(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        _EndTab?.GetComponent<IReset>().ResetAtTheEndOfTheGameEndCoroutine();

        if (photonView.IsMine)
        {
            //_Timer.GameEndSeconds = 0;
            //_Timer.IsGameFinished = false;

            foreach (var iReset in GetComponents<IReset>())
            {
                iReset?.ResetAtTheEndOfTheGameEndCoroutine();
            }
        }
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        _Timer.Seconds = 60;
        _Timer.NightTime = true;
        _Timer.NextPhaseWaitUntil = 0;
        _Timer.WasNextWaitUntilRunning = false;
        _Timer.DayTime = false;
        _Timer.DaysCount = 0;
        _Timer.NightsCount = 0;
        _Timer.HasGameStartVFXInstantiated = false;

        _LostPlayer.HasLostPlayerSet = false;
        _LostPlayer.LostPlayers = new Dictionary<int, bool>();
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        _Timer.GameEndSeconds = 0;
        _Timer.IsGameFinished = false;

        ResetGameEndDictionaries();
    }
    #endregion

    #region InitializeGameEndDataDictionaries
    void InitializeGameEndDataDictionaries()
    {
        if (_GameEndData.PlayersCachedNames == null)
            _GameEndData.PlayersCachedNames = new Dictionary<string, string>();
        if (_GameEndData.PlayersProfilePictureURL == null)
            _GameEndData.PlayersProfilePictureURL = new Dictionary<string, string>();
        if (_GameEndData.PlayersRolesInLastRound == null)
            _GameEndData.PlayersRolesInLastRound = new Dictionary<string, string>();
        if (_GameEndData.PointsForEveryone == null)
            _GameEndData.PointsForEveryone = new Dictionary<string, int>();
        if (_GameEndData.PointsOfTheDoctor == null)
            _GameEndData.PointsOfTheDoctor = new Dictionary<string, int>();
        if (_GameEndData.PointsOfTheInfected == null)
            _GameEndData.PointsOfTheInfected = new Dictionary<string, int>();
        if (_GameEndData.PointsOfTheLizard == null)
            _GameEndData.PointsOfTheLizard = new Dictionary<string, int>();
        if (_GameEndData.PointsOfTheSheriff == null)
            _GameEndData.PointsOfTheSheriff = new Dictionary<string, int>();
        if (_GameEndData.PointsOfTheSoldier == null)
            _GameEndData.PointsOfTheSoldier = new Dictionary<string, int>();

    }
    #endregion

    #region ResetGameEndDictionaries
    void ResetGameEndDictionaries()
    {
        _GameEndData.PlayersCachedNames = new Dictionary<string, string>();
        _GameEndData.PlayersProfilePictureURL = new Dictionary<string, string>();
        _GameEndData.PlayersRolesInLastRound = new Dictionary<string, string>();
        _GameEndData.PointsForEveryone = new Dictionary<string, int>();
        _GameEndData.PointsOfTheDoctor = new Dictionary<string, int>();
        _GameEndData.PointsOfTheInfected = new Dictionary<string, int>();
        _GameEndData.PointsOfTheLizard = new Dictionary<string, int>();
        _GameEndData.PointsOfTheSheriff = new Dictionary<string, int>();
        _GameEndData.PointsOfTheSoldier = new Dictionary<string, int>();
    }
    #endregion

    #region CachePlayersData
    void CachePlayersData()
    {
        if (photonView.IsMine)
        {
            LoopRoleButtonsCallback(RoleButtons =>
            {
                if (!String.IsNullOrEmpty(RoleButtons._OwnerInfo.OwenrUserId))
                {
                    CachePlayersNames(RoleButtons);
                    CachePlayersProfilePicURL(RoleButtons);
                    CachePlayersRoles(RoleButtons);
                }
            });
        }
    }

    void CachePlayersNames(RoleButtonController RoleButtons)
    {
        if (!_GameEndData.PlayersCachedNames.ContainsKey(RoleButtons._OwnerInfo.OwenrUserId))
        {
            _GameEndData.PlayersCachedNames.Add(RoleButtons._OwnerInfo.OwenrUserId, RoleButtons._OwnerInfo.OwnerName);
        }
    }
    void CachePlayersProfilePicURL(RoleButtonController RoleButtons)
    {
        if (!_GameEndData.PlayersProfilePictureURL.ContainsKey(RoleButtons._OwnerInfo.OwenrUserId))
        {
            PlayerBaseConditions.PlayerProfile.ProfilePictureURL(RoleButtons._OwnerInfo.OwenrUserId, URL =>
            {
                _GameEndData.PlayersProfilePictureURL.Add(RoleButtons._OwnerInfo.OwenrUserId, URL);
            });
        }
    }
    void CachePlayersRoles(RoleButtonController RoleButtons)
    {
        if (!String.IsNullOrEmpty(RoleButtons._GameInfo.RoleName) && !_GameEndData.PlayersRolesInLastRound.ContainsKey(RoleButtons._OwnerInfo.OwenrUserId))
        {
            _GameEndData.PlayersRolesInLastRound.Add(RoleButtons._OwnerInfo.OwenrUserId, RoleButtons._GameInfo.RoleName);
        }
    }
    #endregion

    #region DestroyAwardedCards
    void DestroyAwardedCards()
    {
        if(FindObjectOfType<AwardedPlayerCardController>() != null)
        {
            Destroy(FindObjectOfType<AwardedPlayerCardController>().gameObject);
        }
    }
    #endregion

    #region AddOrRemovePoints
    public void AddOrRemovePoints(int senderActorNumber, Dictionary<string, int> PointsDict, int point)
    {
        string dictKey = PlayerBaseConditions.GetRoleButton(senderActorNumber) != null ? PlayerBaseConditions.GetRoleButton(senderActorNumber)._OwnerInfo.OwenrUserId : PhotonNetwork.CurrentRoom.GetPlayer(senderActorNumber).UserId;

        if (PointsDict.ContainsKey(dictKey))
        {
            PointsDict[dictKey] = PointsDict[dictKey] += point;
        }
        else
        {
            PointsDict.Add(dictKey, point);
        }
    }
    #endregion

    #region RewardTheMedic
    void RewardTheMedic()
    {
        int medicsActorNumber = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._GameInfo.RoleName == RoleNames.Medic)._OwnerInfo.OwnerActorNumber;
        AddOrRemovePoints(medicsActorNumber, _GameEndData.PointsOfTheDoctor, 75);
    }
    #endregion

    #region RewardThePlayerForTheRightVote
    void RewardThePlayerForTheRightVote(int lostPlayerActorNumber)
    {
        if (_Timer.NightTime)
        {
            foreach (var item in _GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted)
            {
                string playerWhoHasVotedRolename = PlayerBaseConditions.PlayerRoleName(item.Key);
                string playerAgainstWhomBeenVotedRolename = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerName == item.Value[item.Value.Length - 1])._GameInfo.RoleName;
                int playerAgainstWhomBeenVotedActornumber = Array.Find(_GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerName == item.Value[item.Value.Length - 1])._OwnerInfo.OwnerActorNumber;

                if (playerAgainstWhomBeenVotedActornumber == lostPlayerActorNumber)
                {
                    if (playerWhoHasVotedRolename == RoleNames.Infected || playerWhoHasVotedRolename == RoleNames.Lizard)
                    {
                        if (playerAgainstWhomBeenVotedRolename != RoleNames.Infected && playerAgainstWhomBeenVotedRolename != RoleNames.Lizard)
                        {
                            AddOrRemovePoints(item.Key, _GameEndData.PointsForEveryone, 50);
                        }
                        else
                        {
                            AddOrRemovePoints(item.Key, _GameEndData.PointsForEveryone, -150);
                        }
                    }
                    else if (playerWhoHasVotedRolename != RoleNames.Infected && playerWhoHasVotedRolename != RoleNames.Lizard)
                    {
                        if (playerAgainstWhomBeenVotedRolename == RoleNames.Infected || playerAgainstWhomBeenVotedRolename == RoleNames.Lizard)
                        {
                            AddOrRemovePoints(item.Key, _GameEndData.PointsForEveryone, 50);
                        }
                        else
                        {
                            AddOrRemovePoints(item.Key, _GameEndData.PointsForEveryone, -150);
                        }
                    }
                }
            }
        }
    }
    #endregion
}
