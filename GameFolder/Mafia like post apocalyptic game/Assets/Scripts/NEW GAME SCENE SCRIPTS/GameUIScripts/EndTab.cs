using System;
using UnityEngine;

public class EndTab : MonoBehaviour
{
    [Serializable] public struct UI
    {
        [SerializeField] CanvasGroup canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
        }
    }
    public UI _UI;

    GameManagerEndOfTheGame _GameManagerEndOfTheGame;

    void Awake()
    {
        _GameManagerEndOfTheGame = FindObjectOfType<GameManagerEndOfTheGame>();
    }

    void OnEnable()
    {
        _GameManagerEndOfTheGame._OnRestartTheGame += _OnRestartTheGame;
    }

    void OnDisable()
    {
        _GameManagerEndOfTheGame._OnRestartTheGame -= _OnRestartTheGame;
    }

    void _OnRestartTheGame()
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);
    }
}
