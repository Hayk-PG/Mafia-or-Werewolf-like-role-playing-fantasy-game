using UnityEngine;
using UnityEngine.UI;

public class OnClickMedia : MonoBehaviour
{
    [SerializeField] Button soundButton;
    [SerializeField] Button musicButton;

    [SerializeField] Image soundButtonIcon;
    [SerializeField] Image musicButtonIcon;

    [SerializeField] Sprite[] soundSprites;
    [SerializeField] Sprite[] musicSprites;

    [SerializeField] bool isSoundOn;
    [SerializeField] bool isMusicOn;

    string _Sound  = "Sound";
    string _Music = "Music";


    void Awake()
    {
        isSoundOn = PlayerPrefs.GetInt(_Sound, 1) == 0 ? false: true;
        isMusicOn = PlayerPrefs.GetInt(_Music, 1) == 0 ? false : true;
    }

    void Update()
    {
        OnClickButtons(soundButton);
        OnClickButtons(musicButton);
        Sound();
        Music();
    }

    void OnClickButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == soundButton)
            {
                isSoundOn = !isSoundOn;
                PlayerPrefs.SetInt(_Sound, isSoundOn ? 1 : 0);
            }
            if(button == musicButton)
            {
                isMusicOn = !isMusicOn;
                PlayerPrefs.SetInt(_Music, isMusicOn ? 1 : 0);
            }

            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
        });
    }

    void Sound()
    {
        if (isSoundOn)
        {
            if (soundButtonIcon.sprite != soundSprites[0]) soundButtonIcon.sprite = soundSprites[0];
            if (PlayerBaseConditions.UiSounds.AudioSRC[1].mute != false) PlayerBaseConditions.UiSounds.AudioSRC[1].mute = false;
        }
        else
        {
            if (soundButtonIcon.sprite != soundSprites[1]) soundButtonIcon.sprite = soundSprites[1];
            if (PlayerBaseConditions.UiSounds.AudioSRC[1].mute != true) PlayerBaseConditions.UiSounds.AudioSRC[1].mute = true;
        }
    }

    void Music()
    {
        if (isMusicOn)
        {
            if (musicButtonIcon.sprite != musicSprites[0]) musicButtonIcon.sprite = musicSprites[0];
            if (PlayerBaseConditions.UiSounds.AudioSRC[0].mute != false) PlayerBaseConditions.UiSounds.AudioSRC[0].mute = false;
        }
        else
        {
            if (musicButtonIcon.sprite != musicSprites[1]) musicButtonIcon.sprite = musicSprites[1];
            if (PlayerBaseConditions.UiSounds.AudioSRC[0].mute != true) PlayerBaseConditions.UiSounds.AudioSRC[0].mute = true;
        }
    }
}
