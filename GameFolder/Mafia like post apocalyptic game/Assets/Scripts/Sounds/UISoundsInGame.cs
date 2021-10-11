using UnityEngine;

public class UISoundsInGame : UiSoundsBaseScript
{
    /// <summary>
    /// 0: Music 1: Audio
    /// </summary>
    public override AudioSource[] AudioSRC { get => audioSRC; }
    public override AudioClip[] SoundFX { get => soundFX; }

    [SerializeField] AudioClip[] GameSoundFX;

    
    public override void PlaySoundFX(int index)
    {
        AudioSRC[1].PlayOneShot(SoundFX[index]);
    }

    /// <summary>
    /// 0: ALert 1:Votes 2:Cards rotate 3: Impact 2 soundFX used in Cards rotate 4: End debris 
    /// </summary>
    /// <param name="index"></param>
    public override void PlaySoundFXinGame(int index)
    {
        AudioSRC[1].PlayOneShot(GameSoundFX[index]);
    }
}
