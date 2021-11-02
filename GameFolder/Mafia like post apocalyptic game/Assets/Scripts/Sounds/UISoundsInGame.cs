using System.Collections;
using UnityEngine;

public class UISoundsInGame : UiSoundsBaseScript
{
    /// <summary>
    /// 0: Music 1: Audio
    /// </summary>
    public override AudioSource[] AudioSRC { get => audioSRC; }
    public override AudioClip[] SoundFX { get => soundFX; }

    [SerializeField] AudioClip[] Musics;
    [SerializeField] AudioClip[] GameSoundFX;
    [SerializeField] AudioClip[] AbilitySoundFX;

    
    public override void PlaySoundFX(int index)
    {
        AudioSRC[1].PlayOneShot(SoundFX[index]);
    }

    /// <summary>
    /// 0: ALert 1:Votes 2:Cards rotate 3: Impact 2 soundFX used in Cards rotate 4: End debris 5: Timer 6: Points 7: Points expl 8: Lost 9: Win 10: Firework 11: Selected 12: Info pop up
    /// </summary>
    /// <param name="index"></param>
    public override void PlaySoundFXinGame(int index)
    {
        AudioSRC[1].PlayOneShot(GameSoundFX[index]);
    }

    public override void PlayAblitySoundFX(int index)
    {
        AudioSRC[1].PlayOneShot(AbilitySoundFX[index]);
    }

    #region SwitchMusic
    public void SwitchMusic()
    {
        StartCoroutine(SwitchMusicCoroutine()); 
    }

    IEnumerator SwitchMusicCoroutine()
    {
        yield return null;

        while(AudioSRC[0].volume > 0)
        {
            AudioSRC[0].volume -= 0.5f * Time.deltaTime;
            yield return null;
        }

        AudioSRC[0].volume = 0;
        AudioSRC[0].enabled = false;
        AudioSRC[0].clip = Musics[1];
        AudioSRC[0].enabled = true;

        yield return null;

        while (AudioSRC[0].volume < 0.4)
        {
            AudioSRC[0].volume += 0.5f * Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region SetDefaultMusic
    public void SetDefaultMusic()
    {
        AudioSRC[0].volume = 0.4f;
        AudioSRC[0].enabled = false;
        AudioSRC[0].clip = Musics[0];
        AudioSRC[0].enabled = true;
    }
    #endregion
}
