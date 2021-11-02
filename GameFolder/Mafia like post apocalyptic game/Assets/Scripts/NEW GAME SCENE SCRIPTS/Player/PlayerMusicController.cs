using Photon.Pun;
using UnityEngine;

public class PlayerMusicController : MonoBehaviourPun, IReset
{
    GameManagerTimer _GameManagerTimer { get; set; }
    UISoundsInGame _UISoundsInGame { get; set; }
    internal bool IsMusicSwitched { get; set; }


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
        _UISoundsInGame = FindObjectOfType<UISoundsInGame>();
    }

    void OnEnable()
    {
        _GameManagerTimer.IsResetPhaseActive += SwitchMusic;
    }

    void OnDisable()
    {
        _GameManagerTimer.IsResetPhaseActive -= SwitchMusic;
    }

    #region SwitchMusic
    void SwitchMusic(bool isTrue)
    {
        if (photonView.IsMine && photonView.AmOwner)
        {
            if (isTrue)
            {
                if (!IsMusicSwitched)
                {
                    if (_GameManagerTimer._Timer.NightsCount >= (_GameManagerTimer._GameEndData.PlayersCachedNames.Count > 1 ? _GameManagerTimer._GameEndData.PlayersCachedNames.Count / 2 : 1))
                    {
                        _UISoundsInGame.SwitchMusic();
                        IsMusicSwitched = true;
                    }
                }
            }
        }
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        IsMusicSwitched = false;
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        
    }
    #endregion
}
