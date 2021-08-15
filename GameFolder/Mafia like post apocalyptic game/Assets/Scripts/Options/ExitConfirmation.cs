using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmation : OptionButtonsBaseScript
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
            int index = i;

            Buttons[index].onClick.RemoveAllListeners();
            Buttons[index].onClick.AddListener(() => 
            {
                if (index == 0) OnConfirmExit();
                if (index == 1) OnDenyExit();
            });
        }
    }

    void OnConfirmExit()
    {
        Application.Quit();
    }

    void OnDenyExit()
    {
        Options.instance.OnPressedExitButton(false);
    }



}
