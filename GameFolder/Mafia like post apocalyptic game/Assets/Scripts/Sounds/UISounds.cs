using UnityEngine;

public class UISounds : UiSoundsBaseScript
{
    /// <summary>
    /// 0: Music 1: Audio
    /// </summary>
    public override AudioSource[] AudioSRC { get => audioSRC; }
    public override AudioClip[] SoundFX { get => soundFX; }


    /// <summary>
    /// 0: Button click 1: Back button click 2: Click other UI 3: On InputField value changed
    /// </summary>
    /// <param name="index"></param>
    public override void PlaySoundFX(int index)
    {
        AudioSRC[1].PlayOneShot(SoundFX[index]);
    }
}
