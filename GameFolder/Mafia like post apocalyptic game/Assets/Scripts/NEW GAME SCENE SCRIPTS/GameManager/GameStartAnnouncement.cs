using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using System.Collections;

public class GameStartAnnouncement : MonoBehaviourPun,IReset
{
    static GameStartAnnouncement Master;

    [Serializable] public class Timer
    {
        [SerializeField] Text gameStartAnnouncementText;
        [SerializeField] Text onlinePlayersListText;
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
        public string OnlinePlayersListText
        {
            get => onlinePlayersListText.text;
            set => onlinePlayersListText.text = value;
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
    NetworkCallbacks _NetworkCallbacks;


    void Awake()
    {
        if (photonView.IsMine) Master = this;

        _GameManagerStartTheGame = GetComponent<GameManagerStartTheGame>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _NetworkCallbacks = GetComponent<NetworkCallbacks>();
    }

    void OnEnable()
    {
        _NetworkCallbacks.UpdateOnlinePlayersListCallback += OnUpdateOnlinePlayersList;
    }

    void OnDisable()
    {
        _NetworkCallbacks.UpdateOnlinePlayersListCallback -= OnUpdateOnlinePlayersList;
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

    #region TimerCoroutine
    IEnumerator TimerCoroutine(int currentSecond)
    {
        _Timer.Seconds = currentSecond;

        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length >= (int)PhotonNetwork.CurrentRoom.CustomProperties[RoomCustomProperties.MinRequiredCount] || _Timer.IsMinRequiredCountReached);

        _Timer.IsMinRequiredCountReached = true;

        while (!_Timer.IsTimeToStartTheGame && photonView.IsMine)
        {
            _Timer.Seconds--;
            _Timer.IsTimeToStartTheGame = _Timer.Seconds <= 0 ? true : false;
            _Timer.GameStartAnnouncementText = "Will start in " + "<color=red>" + "<b>"  + "\n" + _Timer.Seconds.ToString("D2") + "</b>" + "</color>" + " seconds";
            if (_Timer.IsTimeToStartTheGame) _GameManagerStartTheGame.StartTheGame();
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region OnUpdateOnlinePlayersList
    void OnUpdateOnlinePlayersList()
    {
        if (!_Timer.IsTimeToStartTheGame)
        {
            _Timer.OnlinePlayersListText = "<color=#ffa500ff>" + PhotonNetwork.PlayerList.Length + "/" + (int)PhotonNetwork.CurrentRoom.CustomProperties[RoomCustomProperties.MinRequiredCount] + "</color>" + "\n";

            foreach (var onlinePlayer in PhotonNetwork.PlayerList)
            {
                _Timer.OnlinePlayersListText += onlinePlayer.NickName + " is <color=#00ff00ff>online...</color>" + "\n";
            }
        }
    }
    #endregion

    #region OnMasterSwitchedOrRejoined
    public void OnMasterSwitchedOrRejoined(bool isPhotonViewMine)
    {
        if(isPhotonViewMine) Master = this;

        if(Master != null) StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        _Timer.Seconds = 60;
        _Timer.IsTimeToStartTheGame = false;
        OnMasterSwitchedOrRejoined(photonView.IsMine);
    }
    #endregion
}
