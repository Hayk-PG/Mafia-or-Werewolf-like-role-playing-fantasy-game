using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayGameController : MonoBehaviour
{
    [Serializable] public class RolesClass
    {
        [SerializeField] int playersCount;
        [SerializeField] bool kingMode;
        [SerializeField] internal SinglePlayRoleButton[] roleButtons;
        [SerializeField] List<string> playersRolesNames = new List<string>(20)
        {
            RoleNames.Citizen, RoleNames.Citizen, RoleNames.Infected, RoleNames.Medic, RoleNames.Infected, RoleNames.Soldier, RoleNames.Infected, RoleNames.Citizen,
            RoleNames.Lizard, RoleNames.Sheriff, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected,
            RoleNames.Citizen, RoleNames.Infected, RoleNames.Soldier, RoleNames.Lizard
        };

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
                    PlayersCount == 8 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 9 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 10 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 11 ? new List<string>(8)
                    {
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Citizen,
                    RoleNames.Medic,
                    RoleNames.Sheriff,
                    RoleNames.HumanKing,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected,
                    RoleNames.Infected
                    } :
                    PlayersCount == 12 ? new List<string>(8)
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
                    RoleNames.Infected,
                    RoleNames.Lizard
                    } :
                    PlayersCount == 13 ? new List<string>(8)
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
                    RoleNames.Lizard
                    } :
                    PlayersCount == 14 ? new List<string>(8)
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
                    PlayersCount == 15 ? new List<string>(8)
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
                    PlayersCount == 16 ? new List<string>(8)
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
                    PlayersCount == 17 ? new List<string>(8)
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
                    PlayersCount == 18 ? new List<string>(8)
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
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 19 ? new List<string>(8)
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
                    RoleNames.Infected,
                    RoleNames.Lizard,
                    RoleNames.MonsterKing
                    } :
                    PlayersCount == 20 ? new List<string>(8)
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
        public GameObject[] PhaseIcons
        {
            get => phaseIcons;
        }
        public IEnumerator TimerCoroutine { get; set; }
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

    public RolesClass _RolesClass;
    public RolesImagesClass _RolesImagesClass;
    public TimerClass _TimerClass;
    public GameClass _GameClass;


    void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        _TimerClass.TimerCoroutine = TimerCoroutine(_TimerClass.Timer, _TimerClass.IsNight);

        SetPlayersRoles();
        StartCoroutine(_TimerClass.TimerCoroutine);
    }

    #region SetPlayersRoles
    void SetPlayersRoles()
    {
        List<int> random = new List<int>();

        for (int i = 0; i < _RolesClass.PlayersCount; i++)
        {
            random.Add(i);
        }

        for (int i = 0; i < _RolesClass.PlayersCount; i++)
        {
            int randomRange = random[UnityEngine.Random.Range(0, random.Count - 1)];
            _RolesClass.roleButtons[i].gameObject.SetActive(true);
            _RolesClass.roleButtons[i].RoleName = _RolesClass.PlayersRolesNames[randomRange];
            _RolesClass.roleButtons[i].RoleImage = i == 0 ? RoleSprites(_RolesClass.PlayersRolesNames[randomRange], 0) : _RolesImagesClass.DefaultSprite[0];
            _RolesClass.roleButtons[i].RoleSprite = RoleSprites(_RolesClass.PlayersRolesNames[randomRange], 0);
            _RolesClass.roleButtons[i].IsPlayer = i == 0 ? true : false;
            _RolesClass.roleButtons[i].IsAlive = true;
            random.Remove(randomRange);
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
                if (IsNightTime)
                {
                    if (IsPhaseResetTime())
                    {
                        Timer(false, 90);
                    }
                }
                else
                {
                    if (IsVoteTime())
                    {
                        LoopRoleButtons(roleButton =>
                        {
                            if (PlayerRoleButton().IsAlive)
                            {
                                if (roleButton.IsPlayer == false && roleButton.IsAlive)
                                {
                                    if (!roleButton._GameObjects.VoteVFX.activeInHierarchy) roleButton._GameObjects.VoteVFX.SetActive(true);
                                }
                            }

                            roleButton.VotesGameObjectActivity(true);
                        });
                    }

                    if (IsPhaseResetTime())
                    {
                        Timer(true, 60);
                    }
                }
            });

            _TimerClass.Timer--;
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region Phases
    void Phases(Action<bool> isNightTime)
    {
        isNightTime?.Invoke(_TimerClass.IsNight);
    }
    #endregion

    #region IsPhaseResetTime
    bool IsPhaseResetTime()
    {
        return _TimerClass.Timer <= 0;
    }
    #endregion

    #region IsVoteTime
    bool IsVoteTime()
    {
        return _TimerClass.Timer <= 30;
    }
    #endregion

    #region Timer
    void Timer(bool isNightTime, int seconds)
    {
        _TimerClass.PhaseIcons[0].SetActive(!isNightTime);
        _TimerClass.PhaseIcons[1].SetActive(isNightTime);
        _TimerClass.Timer = seconds;
        _TimerClass.IsNight = isNightTime;

        if(!isNightTime) _TimerClass.NightsCount++;
        if (isNightTime) _TimerClass.DaysCount++;
    }
    #endregion

    #region PlayerRoleButton
    SinglePlayRoleButton PlayerRoleButton()
    {
        return Array.Find(_RolesClass.roleButtons, roleButton => roleButton.IsPlayer == true);
    }
    #endregion

    #region LoopRoleButtons
    void LoopRoleButtons(Action<SinglePlayRoleButton> RoleButton)
    {
        foreach (var roleButton in _RolesClass.roleButtons)
        {
            RoleButton?.Invoke(roleButton);
        }
    }
    #endregion
}
