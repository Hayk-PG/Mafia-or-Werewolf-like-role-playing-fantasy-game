using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class PlayerSelfTimer : MonoBehaviourPun
{
    public event Action<byte> OnUpdateTimerText;

    [Header("PLAYER ONLINE STATUS")]
    [SerializeField] byte second;
    [SerializeField] byte nightsCount;
    [SerializeField] byte daysCount;
    [SerializeField] bool isNight; 

    public byte Second
    {
        get
        {
            return second;
        }
        set
        {
            second = value;
        }
    }
    public byte NightsCount
    {
        get
        {
            return nightsCount;
        }
        set
        {
            nightsCount = value;
        }
    }
    public byte DaysCount
    {
        get
        {
            return daysCount;
        }
        set
        {
            daysCount = value;
        }
    }
    public bool IsNight
    {
        get
        {
            return isNight;
        }
        set
        {
            isNight = value;
        }
    }


    void Awake()
    {
        IsNight = true;
    }

    void OnEnable()
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnStartPlayerSelfTimer += PlayerEvents_OnStartPlayerSelfTimer;
        }
    }
   
    void OnDisable()
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnStartPlayerSelfTimer -= PlayerEvents_OnStartPlayerSelfTimer;
        }
    }

    void PlayerEvents_OnStartPlayerSelfTimer()
    {
        StartCoroutine(RunTimer());
    }
 
    IEnumerator RunTimer()
    {
        yield return new WaitForSeconds(1);

        if(this == SyncPlayersTimer.GSync.MostSyncedPlayerTimer || SyncPlayersTimer.GSync.MostSyncedPlayerTimer == null)
        {
            if (IsNight && Second < 60 || !IsNight && Second < 90)
            {
                Second++;
            }
            yield return null;

            if (IsNight && Second >= 60)
            {
                Second = 0;
                NightsCount++;
                isNight = false;
            }
            yield return null;

            if (!IsNight && Second >= 90)
            {
                Second = 0;
                DaysCount++;
                isNight = true;
            }
        }
        else
        {
            Second = SyncPlayersTimer.GSync.MostSyncedPlayerTimer.Second;
            NightsCount = SyncPlayersTimer.GSync.MostSyncedPlayerTimer.NightsCount;
            DaysCount = SyncPlayersTimer.GSync.MostSyncedPlayerTimer.DaysCount;
            IsNight = SyncPlayersTimer.GSync.MostSyncedPlayerTimer.IsNight;
        }

        OnUpdateTimerText?.Invoke(Second);

        yield return StartCoroutine(RunTimer());
    }

   












}
