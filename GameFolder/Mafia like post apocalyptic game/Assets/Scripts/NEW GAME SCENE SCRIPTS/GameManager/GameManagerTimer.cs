using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

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
    GameManagerStartTheGame _GameManagerStartTheGame;



    void Awake()
    {
        _Timer.VfxHolder = GetComponent<GameManagerVFXHolder>();
        _UISoundsInGame = FindObjectOfType<UISoundsInGame>();
        _GameManagerStartTheGame = GetComponent<GameManagerStartTheGame>();
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

    internal void RunTimer()
    {
        StartCoroutine(TimerCoroutine(60));
    }

    IEnumerator TimerCoroutine(int currentSeconds)
    {
        CreateTimerStartVFX();

        _Timer.Seconds = currentSeconds;

        while (true)
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

            _UISoundsInGame.PlaySoundFX(0);

            yield return new WaitForSeconds(1);
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

    public void OnMasterSwitchedOrRejoined(bool isPhotonViewMine)
    {
        if (isPhotonViewMine) Master = this;

        if (Master != null) StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }

}
