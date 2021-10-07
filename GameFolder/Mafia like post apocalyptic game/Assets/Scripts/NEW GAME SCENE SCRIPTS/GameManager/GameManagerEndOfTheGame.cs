using UnityEngine;

public class GameManagerEndOfTheGame : MonoBehaviour
{
    public delegate void OnRestartTheGame();
    public OnRestartTheGame _OnRestartTheGame { get; set; }

    public bool StopTheGame;

    void Update()
    {
        if (StopTheGame)
        {
            _OnRestartTheGame?.Invoke();
            StopTheGame = false;
        }
    }
}
