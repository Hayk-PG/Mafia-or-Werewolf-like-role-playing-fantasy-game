using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickResume : MonoBehaviour
{
    [SerializeField] Button resumeButton;

    void Update()
    {
        OnClickResumeButton();
    }

    void OnClickResumeButton()
    {
        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => 
        {
            Options.instance.OnPressedOptionsButtons();
            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
        });
    }
}
