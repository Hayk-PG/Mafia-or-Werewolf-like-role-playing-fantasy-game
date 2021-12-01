using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmation : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    void Update()
    {
        OnClickButton();
    }

    void OnClickButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            buttons[index].onClick.RemoveAllListeners();
            buttons[index].onClick.AddListener(() => 
            {
                if (index == 0) OnConfirmExit();
                if (index == 1) OnDenyExit();
            });
        }
    }

    void OnConfirmExit()
    {
        PlayerBaseConditions.UiSounds.PlaySoundFX(1);
        Application.Quit();
    }

    void OnDenyExit()
    {
        PlayerBaseConditions.UiSounds.PlaySoundFX(1);
        Options.instance.OnPressedExitButton(false);
    }



}
