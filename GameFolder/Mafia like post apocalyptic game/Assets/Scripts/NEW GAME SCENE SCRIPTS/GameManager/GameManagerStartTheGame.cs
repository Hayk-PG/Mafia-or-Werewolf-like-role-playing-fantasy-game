using Photon.Pun;
using System;
using UnityEngine;

public class GameManagerStartTheGame : MonoBehaviourPun
{
    static GameManagerStartTheGame Master;

    [Serializable] public struct GameStart
    {
        [SerializeField] bool gameStarted;

        public bool GameStarted
        {
            get => gameStarted;
            set => gameStarted = value;
        }
    }

    public GameStart _GameStart;
    GameStartAnnouncement _GameStartAnnouncement;
    GameManagerTimer _GameManagerTimer;
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;
    GameManagerPlayerVotesController _GameManagerPlayerVotesController;


    void Awake()
    {
        _GameStartAnnouncement = GetComponent<GameStartAnnouncement>();
        _GameManagerTimer = GetComponent<GameManagerTimer>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
    }

    public void StartTheGame()
    {
        if (_GameStartAnnouncement._Timer.IsTimeToStartTheGame && photonView.IsMine)
        {
            _GameManagerTimer.RunTimer();

            if(!_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet) _GameManagerSetPlayersRoles.SetPlayersRoles();

           // _GameManagerPlayerVotesController.TransferPlayersVotesToTheNewMaster();

            _GameStart.GameStarted = true;
        }
    }

    public void OnMasterSwitchedOrRejoined(bool isPhotonViewMine)
    {
        if (isPhotonViewMine) Master = this;

        if (Master != null) StartTheGame();
    }
}
