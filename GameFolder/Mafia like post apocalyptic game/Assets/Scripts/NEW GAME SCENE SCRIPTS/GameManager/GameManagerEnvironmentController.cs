using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerEnvironmentController : MonoBehaviourPun, IReset
{
    [Serializable] struct UI
    {
        [SerializeField] internal Image dayBackground;
        [SerializeField] internal Image nightBackground;
    }
    [Serializable] struct Particles
    {

    }

    [SerializeField] UI _UI;
    [SerializeField] Particles _Particles;

    GameManagerTimer _GameManagerTimer { get; set; }

    [SerializeField] float nightBGalpha;


    void Awake()
    {
        _GameManagerTimer = GetComponent<GameManagerTimer>();
    }

    void Update()
    {
        if (_GameManagerTimer._Timer.DayTime)
        {
            Day();
        }
        else if(_GameManagerTimer._Timer.NightTime && _GameManagerTimer._Timer.NightsCount > 0)
        {
            Night();
        }
    }

    #region Day
    void Day()
    {
        if (_GameManagerTimer._Timer.Seconds >= 85)
        {
            if (nightBGalpha > 0)
            {
                nightBGalpha -= 50 * Time.deltaTime;
                _UI.nightBackground.color = new Color32(255, 255, 255, (byte)nightBGalpha);
            }
        }
        else
        {
            if (_UI.nightBackground.color.a > 0) _UI.nightBackground.color = new Color32(255, 255, 255, 0);
        }
    }
    #endregion

    #region Night
    void Night()
    {
        if (_GameManagerTimer._Timer.Seconds >= 55)
        {
            if (nightBGalpha < 255)
            {
                nightBGalpha += 50 * Time.deltaTime;
                _UI.nightBackground.color = new Color32(255, 255, 255, (byte)nightBGalpha);
            }
        }
        else
        {
            if (_UI.nightBackground.color.a < 255) _UI.nightBackground.color = new Color32(255, 255, 255, 255);
        }
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        nightBGalpha = 255;
        _UI.nightBackground.color = new Color32(255, 255, 255, 255);
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        
    }
    #endregion
}
