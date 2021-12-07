using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayGameController : MonoBehaviour, ISinglePlayReset
{
    public event Action<bool> OnPhaseReset;
    public event Action OnAiDayVote;
    public event Action OnAiNightVote;

    [Serializable] public class RolesClass
    {
        [SerializeField] int playersCount;
        [SerializeField] bool kingMode;
        [SerializeField] List<SinglePlayRoleButton> roleButtons;
        [SerializeField] List<string> playersRolesNames = new List<string>(20)
        {
            RoleNames.Citizen, RoleNames.Citizen, RoleNames.Infected, RoleNames.Medic, RoleNames.Infected, RoleNames.Soldier, RoleNames.Infected, RoleNames.Citizen,
            RoleNames.Lizard, RoleNames.Sheriff, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected,
            RoleNames.Citizen, RoleNames.Infected, RoleNames.Soldier, RoleNames.Lizard
        };
        [SerializeField] List<string> malePlayersNames;
        [SerializeField] List<string> femalePlayersNames;

        public List<SinglePlayRoleButton> RoleButtons
        {
            get => roleButtons;
        }      
        public int PlayersCount
        {
            get => playersCount;
            set => playersCount = value;
        }
        public bool KingMode
        {
            get => kingMode;
        }
        public List<string> PlayersRolesNames
        {
            get
            {
                if (KingMode)
                {
                    return                    
                    PlayersCount == 4 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Infected
                    } :
                    PlayersCount == 5 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Infected
                    } :
                    PlayersCount == 6 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 7 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 8 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 9 ? new List<string>(9)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 10 ? new List<string>(10)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard
                    } :
                    PlayersCount == 11 ? new List<string>(11)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 12 ? new List<string>(12)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 13 ? new List<string>(13)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 14 ? new List<string>(14)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 15 ? new List<string>(15)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 16 ? new List<string>(16)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 17 ? new List<string>(17)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 18 ? new List<string>(18)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 19 ? new List<string>(19)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 20 ? new List<string>(20)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.Soldier,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } : playersRolesNames;
                }
                else
                {
                    return playersRolesNames;
                }
            }
        }
        public List<string> MalePlayersNames
        {
            get => malePlayersNames;
        }
        public List<string> FemalePlayersNames
        {
            get => femalePlayersNames;
        }
    }
    [Serializable] public class RolesImagesClass
    {
        [SerializeField] Sprite[] citizenRoleSprites;
        [SerializeField] Sprite[] doctorRoleSprites;
        [SerializeField] Sprite[] sheriffRoleSprites;
        [SerializeField] Sprite[] soldierRoleSprite;
        [SerializeField] Sprite[] infectedRoleSprites;
        [SerializeField] Sprite[] witcherRoleSprite;
        [SerializeField] Sprite[] humanKingSprite;
        [SerializeField] Sprite[] monsterKingSprite;
        [SerializeField] Sprite[] defaultSprites;

        public Sprite[] CitizenRoleSprites
        {
            get => citizenRoleSprites;
        }
        public Sprite[] DoctorRoleSprites
        {
            get => doctorRoleSprites;
        }
        public Sprite[] SheriffRoleSprites
        {
            get => sheriffRoleSprites;
        }
        public Sprite[] SoldierRoleSprite
        {
            get => soldierRoleSprite;
        }
        public Sprite[] InfectedRoleSprites
        {
            get => infectedRoleSprites;
        }
        public Sprite[] WitcherRoleSprites
        {
            get => witcherRoleSprite;
        }
        public Sprite[] HumanKing
        {
            get => humanKingSprite;
        }
        public Sprite[] MonsterKing
        {
            get => monsterKingSprite;
        }
        public Sprite[] DefaultSprite
        {
            get => defaultSprites;
        }
    }
    [Serializable] public class TimerClass
    {
        [SerializeField] Text timerText;
        [SerializeField] bool isNight;
        [SerializeField] bool proccesing;
        [SerializeField] int nightsCount;
        [SerializeField] int daysCount;
        [SerializeField] GameObject[] phaseIcons;

        public int Timer
        {
            get => int.Parse(timerText.text);
            set => timerText.text = value.ToString();
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
        public bool IsNight
        {
            get => isNight;
            set => isNight = value;
        }
        public bool Proccesing
        {
            get => proccesing;
            set => proccesing = value;
        }
        public GameObject[] PhaseIcons
        {
            get => phaseIcons;
        }
        public IEnumerator TimerCoroutine { get; set; }
    }
    [Serializable] public class TeamsClass
    {
        [SerializeField] Text humansTeamText;
        [SerializeField] Text monstersTeamText;

        public int HumansTeamCount
        {
            get => int.Parse(humansTeamText.text);
            set => humansTeamText.text = value.ToString();
        }
        public int MonstersTeamCount
        {
            get => int.Parse(monstersTeamText.text);
            set => monstersTeamText.text = value.ToString();
        }
    }
    [Serializable] public class GameClass
    {
        [SerializeField] bool isGameStarted;

        public bool IsGameStarted
        {
            get => isGameStarted;
            set => isGameStarted = value;
        }
    }
    [Serializable] public class AI
    {
        [SerializeField] bool isAiVoteEventInvoked;

        public bool IsAiVoteEventInvoked
        {
            get => isAiVoteEventInvoked;
            set => isAiVoteEventInvoked = value;
        }
    }
    [Serializable] public class Players
    {
        public List<SinglePlayRoleButton> ActivePlayers { get; set; } = new List<SinglePlayRoleButton>();
        public List<SinglePlayRoleButton> LostPlayers { get; set; } = new List<SinglePlayRoleButton>();
    }

    public RolesClass _RolesClass;
    public RolesImagesClass _RolesImagesClass;
    public TimerClass _TimerClass;
    public TeamsClass _TeamsClass;
    public GameClass _GameClass;
    public AI _AI;
    public Players _Players;

    SinglePlayerGameSettings _SPGS { get; set; }
    SinglePlayerRoleButtonsContainersController _SPRBCC { get; set; }
    SinglePlayerRolesSetController _SPRSC { get; set; }
    SinglePlayerPoints _SPP { get; set; }

    public bool test;


    void Awake()
    {
        _SPGS = GetComponent<SinglePlayerGameSettings>();
        _SPRBCC = GetComponent<SinglePlayerRoleButtonsContainersController>();
        _SPRSC = GetComponent<SinglePlayerRolesSetController>();

        _SPP = FindObjectOfType<SinglePlayerPoints>();
    }

    void OnEnable()
    {
        _SPGS.OnStartTheGame += _SPGS_OnStartTheGame;
    }
  
    void OnDisable()
    {
        _SPGS.OnStartTheGame -= _SPGS_OnStartTheGame;
    }

    void _SPGS_OnStartTheGame(SinglePlayerGameSettings.GameData GameData)
    {
        StopTimerCoroutine();
        GetGameData(GameData);
        SetPlayersRoles();
        StartCoroutine(_TimerClass.TimerCoroutine);
    }

    #region StopTimerCoroutine
    void StopTimerCoroutine()
    {
        if(_TimerClass.TimerCoroutine != null)
            StopCoroutine(_TimerClass.TimerCoroutine);
        else
            _TimerClass.TimerCoroutine = TimerCoroutine(_TimerClass.Timer, _TimerClass.IsNight);
    }
    #endregion

    #region GetGameData
    void GetGameData(SinglePlayerGameSettings.GameData GameData)
    {
        _RolesClass.PlayersCount = GameData.PlayersCount;
    }
    #endregion

    #region SetPlayersRoles
    void SetPlayersRoles()
    {
        _SPRSC.HideAllRoleButtons(new SinglePlayerRolesSetController.RolesInfo { _RolesClass = _RolesClass});
        _SPRSC.GettingRandomNumber(out List<int> random);

        for (int i = 0; i < _RolesClass.PlayersCount; i++)
        {
            _SPRSC.SettingRandomRange(new SinglePlayerRolesSetController.RolesInfo { Index = i}, random, out int randomRange);
            _SPRSC.SettingRandomNames(_RolesClass, out string randomName);
            _SPRSC.SettingRoles(new SinglePlayerRolesSetController.RolesInfo(_RolesClass, _RolesImagesClass, i, randomRange, randomName, RoleSprites(_RolesClass.PlayersRolesNames[randomRange], 0)));
            _SPRSC.IfPlayerIsInfected(new SinglePlayerRolesSetController.RolesInfo { _RolesClass = _RolesClass, Index = i, RoleSprite = RoleSprites(RoleNames.Infected, 0) });
            _SPRSC.DefineRoleNames(new SinglePlayerRolesSetController.RolesInfo { _RolesClass = _RolesClass, Index = i });
            _SPRSC.RemoveRandoms(random, new SinglePlayerRolesSetController.RolesInfo { RandomRange = randomRange});
            _SPRSC.RemoveMaleNames(new SinglePlayerRolesSetController.RolesInfo { _RolesClass = _RolesClass, RandomName = randomName });

            AddingActivePlayers(_RolesClass.RoleButtons[i]);
            SetTeams(i);
        }
    }
   
    Sprite RoleSprites(string roleName, int gender)
    {
        Sprite roleSprite = null;

        switch (roleName)
        {
            case RoleNames.Citizen: roleSprite = _RolesImagesClass.CitizenRoleSprites[gender]; break;
            case RoleNames.Medic: roleSprite = _RolesImagesClass.DoctorRoleSprites[gender]; break;
            case RoleNames.Sheriff: roleSprite = _RolesImagesClass.SheriffRoleSprites[gender]; break;
            case RoleNames.Soldier: roleSprite = _RolesImagesClass.SoldierRoleSprite[gender]; break;
            case RoleNames.HumanKing: roleSprite = _RolesImagesClass.HumanKing[gender]; break;
            case RoleNames.Infected: roleSprite = _RolesImagesClass.InfectedRoleSprites[gender]; break;
            case RoleNames.Lizard: roleSprite = _RolesImagesClass.WitcherRoleSprites[gender]; break;
            case RoleNames.MonsterKing: roleSprite = _RolesImagesClass.MonsterKing[gender]; break;
        }

        return roleSprite;
    }
    #endregion

    #region TimerCoroutine
    IEnumerator TimerCoroutine(int second, bool isNight)
    {
        _TimerClass.IsNight = isNight;
        _TimerClass.Timer = second;

        while (_GameClass.IsGameStarted)
        {
            Phases(IsNightTime =>
            {
                if (!_TimerClass.Proccesing)
                {
                    if (IsNightTime)
                    {
                        if (SinglePlayGlobalConditions.IsVoteTime())
                        {
                            NightVote();
                        }

                        if (SinglePlayGlobalConditions.IsPhaseResetTime())
                        {      
                            StartCoroutine(GetLostPlayer(LostPlayer =>
                            {
                                Timer(false, 90);
                                OnPhaseReset?.Invoke(false);

                                if (LostPlayer != null && !LostPlayer.IsHealed)
                                {
                                    LostPlayer.Lost();
                                    UpdateTeams(LostPlayer);
                                    _Players.ActivePlayers.Remove(LostPlayer);
                                    _Players.LostPlayers.Add(LostPlayer);
                                    _SPRBCC.OnPlayerLost(LostPlayer.transform);
                                    _SPP.PointsForStayingAlive(new SinglePlayerPoints.Data(PlayerRoleButton(), null, 10, 0));
                                }
                                _TimerClass.Proccesing = false;
                            }, 
                            KilledPlayerByKnight => 
                            {
                                if (KilledPlayerByKnight != null && !KilledPlayerByKnight.IsHealed)
                                {
                                    KilledPlayerByKnight.Lost();
                                    UpdateTeams(KilledPlayerByKnight);
                                    _Players.ActivePlayers.Remove(KilledPlayerByKnight);
                                    _Players.LostPlayers.Add(KilledPlayerByKnight);
                                    _SPRBCC.OnPlayerLost(KilledPlayerByKnight.transform);
                                }
                            }));
                        }
                    }
                    else
                    {
                        if (SinglePlayGlobalConditions.IsVoteTime())
                        {
                            DayVote();
                        }

                        if (SinglePlayGlobalConditions.IsPhaseResetTime())
                        {
                            StartCoroutine(GetLostPlayer(LostPlayer =>
                            {
                                Timer(true, 60);
                                OnPhaseReset?.Invoke(true);

                                if (LostPlayer != null)
                                {
                                    LostPlayer.Lost();
                                    UpdateTeams(LostPlayer);
                                    _Players.ActivePlayers.Remove(LostPlayer);
                                    _Players.LostPlayers.Add(LostPlayer);
                                    _SPRBCC.OnPlayerLost(LostPlayer.transform);
                                    _SPP.PointsForStayingAlive(new SinglePlayerPoints.Data(PlayerRoleButton(), null, 10, 0));
                                }
                                _TimerClass.Proccesing = false;
                            }, null));
                        }
                    }
                }
            });

            if (_TimerClass.Timer > 0)
                _TimerClass.Timer--;
            else _TimerClass.Timer = 0;

            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region NightVote
    void NightVote()
    {
        LoopRoleButtons(roleButton =>
        {
            if (SinglePlayGlobalConditions.CanParticipateInNightVote())
            {
                roleButton.VotesIconActivity(roleButton.IsAlive);
            }

            roleButton.VotesCountTextObjActivity(SinglePlayGlobalConditions.AmIInfected());
        });

        OnAiNightVote?.Invoke();
    }
    #endregion

    #region DayVote
    void DayVote()
    {
        LoopRoleButtons(roleButton =>
        {
            if (SinglePlayGlobalConditions.CanParticipateInDayVote())
            {
                roleButton.VotesIconActivity(roleButton.IsAlive);
            }

            roleButton.VotesCountTextObjActivity(true);
        });

        OnAiDayVote?.Invoke();
    }
    #endregion

    #region Phases
    void Phases(Action<bool> isNightTime)
    {
        isNightTime?.Invoke(_TimerClass.IsNight);
    }
    #endregion

    #region Timer
    void Timer(bool isNightTime, int seconds)
    {
        _TimerClass.PhaseIcons[0].SetActive(!isNightTime);
        _TimerClass.PhaseIcons[1].SetActive(isNightTime);
        _TimerClass.Timer = seconds;
        _TimerClass.IsNight = isNightTime;

        if (!isNightTime) _TimerClass.NightsCount++;
        if (isNightTime) _TimerClass.DaysCount++;
    }
    #endregion

    #region PlayerRoleButton
    public SinglePlayRoleButton PlayerRoleButton()
    {
        return Array.Find(_RolesClass.RoleButtons.ToArray(), roleButton => roleButton.IsPlayer == true);
    }
    #endregion

    #region LoopRoleButtons
    public void LoopRoleButtons(Action<SinglePlayRoleButton> RoleButton)
    {
        foreach (var roleButton in _RolesClass.RoleButtons)
        {
            RoleButton?.Invoke(roleButton);
        }
    }
    #endregion

    #region ActivePlayers
    void AddingActivePlayers(SinglePlayRoleButton activePlayer)
    {
        _Players.ActivePlayers.Add(activePlayer);
    }
    #endregion

    #region GetLostPlayerCoroutine
    IEnumerator GetLostPlayer(Action<SinglePlayRoleButton> LostPlayerIsSet, Action<SinglePlayRoleButton> KilledPlayerByKnight)
    {
        _TimerClass.Proccesing = true;

        SinglePlayRoleButton killedPlayerByKnight = null;

        List<int> playersVotesCount = new List<int>();
        List<SinglePlayRoleButton> highestVotesCountPlayers = new List<SinglePlayRoleButton>();
        SinglePlayRoleButton lostPlayer = null;

        foreach (var roleButton in _RolesClass.RoleButtons)
        {
            if (roleButton.Name != null)
            {
                playersVotesCount.Add(roleButton.VotesCount);
            }

            yield return null;
        }

        playersVotesCount.Sort();
        yield return null;

        if (playersVotesCount[playersVotesCount.Count - 1] > 0)
        {
            foreach (var roleButton in _RolesClass.RoleButtons)
            {
                if (roleButton.Name != null && roleButton.VotesCount >= playersVotesCount[playersVotesCount.Count - 1])
                {
                    highestVotesCountPlayers.Add(roleButton);
                }

                yield return null;
            }

            int randomIndex = UnityEngine.Random.Range(0, highestVotesCountPlayers.Count);
            lostPlayer = highestVotesCountPlayers[randomIndex];
        }

        yield return null;

        LoopRoleButtons(RoleButton => 
        {
            if(RoleButton.IsAlive && RoleButton.IsKilledByKnight)
            {
                killedPlayerByKnight = RoleButton;
            }
        });

        yield return new WaitForSeconds(1);

        LostPlayerIsSet?.Invoke(lostPlayer);
        KilledPlayerByKnight?.Invoke(killedPlayerByKnight);
    }
    #endregion

    #region UpdateTeams
    void SetTeams(int i)
    {
        if (SinglePlayGlobalConditions.IsPlayerInHumansTeam(_RolesClass.RoleButtons[i]))
        {
            _TeamsClass.HumansTeamCount++;
        }
        else
        {
            _TeamsClass.MonstersTeamCount++;
        }
    }

    void UpdateTeams(SinglePlayRoleButton roleButton)
    {
        if (SinglePlayGlobalConditions.IsPlayerInHumansTeam(roleButton))
        {
            _TeamsClass.HumansTeamCount--;
        }
        else
        {
            _TeamsClass.MonstersTeamCount--;
        }
    }
    #endregion

    #region IReset
    public void Reset()
    {
        _Players.ActivePlayers = new List<SinglePlayRoleButton>();
        _Players.LostPlayers = new List<SinglePlayRoleButton>();
    }
    #endregion
}
