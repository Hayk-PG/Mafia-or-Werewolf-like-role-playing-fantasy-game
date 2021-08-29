﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerTimer : MonoBehaviourPun
{
    static GameManagerTimer Master;

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

        [HideInInspector] public GameManagerVFXHolder VfxHolder;
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
    UISoundsInGame _UISoundsInGame;

    const byte CreateVFX_Event = 0;
    const byte PlaySoundFX_Event = 1;

    void Awake()
    {
        _Timer.VfxHolder = GetComponent<GameManagerVFXHolder>();
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
        if(_Timer.Moon.activeInHierarchy != _Timer.NightTime) _Timer.Moon.SetActive(_Timer.NightTime);
        if(_Timer.Sun.activeInHierarchy != _Timer.DayTime) _Timer.Sun.SetActive(_Timer.DayTime);
    }

    void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code == CreateVFX_Event)
        {
            object[] datas = (object[])obj.CustomData;

            if ((string)datas[0] == "CreateVFX") CreateTimerStartVFX();
        }
        if (obj.Code == PlaySoundFX_Event)
        {
            object[] datas = (object[])obj.CustomData;

            if ((string)datas[1] == "PlaySoundFX") _UISoundsInGame.PlaySoundFX(0);
        }
    }

    internal void RunTimer()
    {
        StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }

    IEnumerator TimerCoroutine(int currentSeconds)
    {
        while (photonView.IsMine)
        {
            _Timer.Seconds = currentSeconds;

            object[] datas = new object[] { "CreateVFX", "PlaySoundFX" };

            PhotonNetwork.RaiseEvent(CreateVFX_Event, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);

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
                _Timer.TimerText = _Timer.Seconds.ToString();

                PhotonNetwork.RaiseEvent(PlaySoundFX_Event, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendUnreliable);

                yield return new WaitForSeconds(1);
            }
        }
    }

    void CreateTimerStartVFX()
    {
        if (!_Timer.HasGameStartVFXInstantiated)
        {
            _Timer.VfxHolder.CreateVFX(0);
            _Timer.HasGameStartVFXInstantiated = true;
        }
    }   
}
