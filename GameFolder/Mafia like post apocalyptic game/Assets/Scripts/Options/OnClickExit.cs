using UnityEngine;
using UnityEngine.UI;

public class OnClickExit : OptionButtonsBaseScript
{
    protected override Button[] Buttons => buttons;



    protected override void Update()
    {
        OnClickButton();
    }

    protected override void OnClickButton()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].onClick.RemoveAllListeners();
            Buttons[i].onClick.AddListener(() => 
            {
                PlayerBaseConditions.UiSounds.PlaySoundFX(0);
                Options.instance.OnPressedExitButton(true);
            });
        }
    }   
}
