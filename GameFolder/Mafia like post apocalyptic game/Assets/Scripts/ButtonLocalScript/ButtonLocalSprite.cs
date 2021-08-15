using UnityEngine;
using UnityEngine.UI;

public class ButtonLocalSprite : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image[] buttonImages;

    [Header("SPRITES")]
    [SerializeField] Sprite on;
    [SerializeField] Sprite off;

    [Header("COLOR")]
    [SerializeField] Color[] buttonColor;

    Image[] ButtonImages
    {
        get
        {
            return buttonImages;
        }
        set
        {
            buttonImages = value;
        }
    }


    public void OnClickSwitchButton(bool isOn)
    {
        if (isOn)
        {
            ButtonImages[0].color = buttonColor[1];
            ButtonImages[1].sprite = on;
        }
        else
        {
            ButtonImages[0].color = buttonColor[0];
            ButtonImages[1].sprite = off;
        }
    }

    public void OnClickGenderButtons(bool isClicked)
    {
        ButtonImages[0].color = isClicked ? buttonColor[1] : buttonColor[0];
    }
}
