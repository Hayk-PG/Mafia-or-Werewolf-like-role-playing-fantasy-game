using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using System.Collections;

public class GameStartAnnouncement : MonoBehaviourPun
{
    static GameStartAnnouncement Master;

    [Serializable] public class Timer
    {
        [SerializeField] Text gameStartAnnouncementText;
        [SerializeField] GameObject gameStartAnnouncementScreenObj;
        [SerializeField] GameObject gameStartAnnouncementTextObj;
        [SerializeField] int seconds;
        [SerializeField] bool isTimeToStartTheGame;
        [SerializeField] bool isMinRequiredCountReached;

        public string GameStartAnnouncementText
        {
            get => gameStartAnnouncementText.text;
            set => gameStartAnnouncementText.text = value;
        }
        public GameObject GameStartAnnouncementScreenObj
        {
            get => gameStartAnnouncementScreenObj;
        }
        public GameObject GameStartAnnouncementTextObj
        {
            get => gameStartAnnouncementTextObj;
        }      
        public int Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        public bool IsTimeToStartTheGame
        {
            get => isTimeToStartTheGame;
            set => isTimeToStartTheGame = value;
        }
        public bool IsMinRequiredCountReached
        {
            get => isMinRequiredCountReached;
            set => isMinRequiredCountReached = value;
        }
    }

    public Timer _Timer;
    GameManagerStartTheGame _GameManagerStartTheGame;
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;


    void Awake()
    {
        if (photonView.IsMine) Master = this;

        _GameManagerStartTheGame = GetComponent<GameManagerStartTheGame>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
    }

    void Start()
    {
        if(Master != null) StartCoroutine(TimerCoroutine(60));
    }

    void Update()
    {
        if (_Timer.GameStartAnnouncementScreenObj.activeInHierarchy != !_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet) _Timer.GameStartAnnouncementScreenObj.SetActive(!_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet);
        if (_Timer.GameStartAnnouncementTextObj.activeInHierarchy != !_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet) _Timer.GameStartAnnouncementTextObj.SetActive(!_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet);
    }

    IEnumerator TimerCoroutine(int currentSecond)
    {
        _Timer.Seconds = currentSecond;

        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length >= (int)PhotonNetwork.CurrentRoom.CustomProperties[RoomCustomProperties.MinRequiredCount] || _Timer.IsMinRequiredCountReached);

        _Timer.IsMinRequiredCountReached = true;

        while (!_Timer.IsTimeToStartTheGame && photonView.IsMine)
        {
            _Timer.Seconds--;
            _Timer.IsTimeToStartTheGame = _Timer.Seconds <= 0 ? true : false;
            _Timer.GameStartAnnouncementText = "Will start in " + _Timer.Seconds.ToString("D2") + " seconds";
            if (_Timer.IsTimeToStartTheGame) _GameManagerStartTheGame.StartTheGame();
            yield return new WaitForSeconds(1);
        }
    }

    public void OnMasterSwitchedOrRejoined(bool isPhotonViewMine)
    {
        if(isPhotonViewMine) Master = this;

        if(Master != null) StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }
}
