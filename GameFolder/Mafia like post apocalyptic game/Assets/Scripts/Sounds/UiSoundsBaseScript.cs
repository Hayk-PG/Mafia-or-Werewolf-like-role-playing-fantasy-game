using System;
using UnityEngine;

public abstract class UiSoundsBaseScript : MonoBehaviour
{
    [SerializeField] protected AudioSource[] audioSRC;
    [SerializeField] protected AudioClip[] soundFX;
   
    public abstract AudioSource[] AudioSRC { get; }
    public abstract AudioClip[] SoundFX { get; }

    public abstract void PlaySoundFX(int index);

    /// <summary>
    /// 0: ALert 1:Votes 2:Cards rotate 3: Impact 2 soundFX used in Cards rotate 4: End debris 5: Timer 6: Points 7: Points expl 8: Lost 9: Win 10: Firework 11: Selected 12: Info pop up
    /// </summary>
    /// <param name="index"></param>
    public virtual void PlaySoundFXinGame(int index)
    {

    }

    public virtual void PlayAblitySoundFX(int index)
    {

    }
}
