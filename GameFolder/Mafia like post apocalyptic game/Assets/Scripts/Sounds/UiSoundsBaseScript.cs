using System;
using UnityEngine;

public abstract class UiSoundsBaseScript : MonoBehaviour
{
    [SerializeField] protected AudioSource[] audioSRC;
    [SerializeField] protected AudioClip[] soundFX;
   
    public abstract AudioSource[] AudioSRC { get; }
    public abstract AudioClip[] SoundFX { get; }

    public abstract void PlaySoundFX(int index);

    public virtual void PlaySoundFXinGame(int index)
    {

    }
}
