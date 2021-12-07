using System;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerGameSettings : MonoBehaviour
{
    internal event Action<GameData> OnStartTheGame;
    internal class GameData
    {
        internal int PlayersCount { get; set; }

        internal GameData(int PlayersCount)
        {
            this.PlayersCount = PlayersCount;
        }
    }

    [Serializable] internal class Players
    {
        [SerializeField] Text playersCountText;

        internal int PlayersCount
        {
            get => int.Parse(playersCountText.text);
            set => playersCountText.text = value.ToString();
        }

    }
    [Serializable] internal class UI
    {
        [SerializeField] internal Button confirmButton;
        [SerializeField] internal CanvasGroup canvasGroup;
    }


    [SerializeField] internal Players _Players;
    [SerializeField] internal UI _UI;


    void Update()
    {
        OnConfirmButton();
    }

    void OnConfirmButton()
    {
        _UI.confirmButton.onClick.RemoveAllListeners();
        _UI.confirmButton.onClick.AddListener(StartTheGame);
    }

    void StartTheGame()
    {
        OnStartTheGame?.Invoke(new GameData(_Players.PlayersCount));
        VFXCamera.VFXCameraActivity(true);
        MyCanvasGroups.CanvasGroupActivity(_UI.canvasGroup, false);
    }

    public void OnSliderValue(Slider slider)
    {
        _Players.PlayersCount = (int)slider.value;
    }
}
