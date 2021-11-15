using System;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayGameSpeedController : MonoBehaviour
{
    public enum Speed { Normal, TwoTimesFaster, ThreeTimesFaster, FiveTimesFaster, TenTimesFaster }
    public Speed speed;

    [Serializable] public class Playback
    {
        [SerializeField] Button playbackButton;
        [SerializeField] Text gameSpeedText;

        public Button PlaybackButton
        {
            get => playbackButton;
        }
        public string GameSpeed
        {
            get => gameSpeedText.text;
            set => gameSpeedText.text = value;
        }
    }

    public Playback _Playback;

    void Update()
    {
        _Playback.PlaybackButton.onClick.RemoveAllListeners();
        _Playback.PlaybackButton.onClick.AddListener(GameSpeed);
    }

    void GameSpeed()
    {
        int p = (int)speed;

        if ((int)speed < 4)
        {
            p++;
        }
        else
        {
            p = 0;
        }

        speed = (Speed)p;
        _Playback.GameSpeed = speed == Speed.Normal ? "x1" : speed == Speed.TwoTimesFaster ? "x2" : speed == Speed.ThreeTimesFaster ? "x3" : speed == Speed.FiveTimesFaster ? "x5" : "x10";
        int index = _Playback.GameSpeed.IndexOf("x");
        int length = _Playback.GameSpeed.Length - 1;
        Time.timeScale = int.Parse(_Playback.GameSpeed.Substring(1, length));
    }
}
