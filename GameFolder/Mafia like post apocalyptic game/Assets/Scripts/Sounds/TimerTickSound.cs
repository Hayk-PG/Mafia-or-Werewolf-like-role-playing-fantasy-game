using UnityEngine;

public class TimerTickSound : MonoBehaviour
{
    UISoundsInGame _UISoundsInGame;

    void Awake()
    {
        _UISoundsInGame = GetComponent<UISoundsInGame>();
    }

    public void PlayTimerTickingSoundFX(ExitGames.Client.Photon.EventData obj, GameManagerTimer.Timer _Timer)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[1] == RaiseEventsStrings.PlayTimerTickingSoundFX)
        {
            if (_Timer.Seconds <= 10) _UISoundsInGame.PlaySoundFX(0);
        }
    }
}
