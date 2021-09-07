using System;
using UnityEngine;

public class UISoundsInGame : UISounds
{
    [Serializable] public struct Sounds
    {
       [SerializeField] AudioClip[] soundFX;      
        
        /// <summary>
        /// 0: Timer Pop up 
        /// </summary>
        public AudioClip[] SoundFX
        {
            get => soundFX;
        }
    }

    public Sounds _Sounds;

    public void PlaySoundFX(int index)
    {
        uiSRC.PlayOneShot(_Sounds.SoundFX[index]);
    }
}
