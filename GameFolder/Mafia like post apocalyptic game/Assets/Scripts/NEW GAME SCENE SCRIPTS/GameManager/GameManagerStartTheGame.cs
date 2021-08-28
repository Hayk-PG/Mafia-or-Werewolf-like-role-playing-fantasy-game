using System;
using UnityEngine;

public class GameManagerStartTheGame : MonoBehaviour
{
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
    GameManagerTimer _GameManagerTimer;
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;


    void Awake()
    {
        _GameManagerTimer = GetComponent<GameManagerTimer>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
    }

    public void StartTheGame()
    {
        _GameManagerTimer.RunTimer();
        _GameManagerSetPlayersRoles.SetPlayersRoles();

        _GameStart.GameStarted = true;
    }
}
