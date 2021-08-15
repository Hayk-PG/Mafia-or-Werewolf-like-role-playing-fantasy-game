using UnityEngine;
using UnityEngine.UI;

public class ConfirmPlayerInfo : MonoBehaviour
{
    enum ToggleEnum { None, ToggleMale, ToggleFemale}
    ToggleEnum toggleEnum;

    bool isTogglePressed;

    [Header("UI")]
    [SerializeField] InputField playerNameInputField;
    [SerializeField] Toggle[] toggles;
    [SerializeField] CanvasGroup tabCG;

    public bool IsCanvasGroupActive
    {
        get
        {
            return tabCG.interactable;
        }
    }
    public bool IsNameSet
    {
        get
        {
            if(string.IsNullOrEmpty(playerNameInputField.text) || string.IsNullOrWhiteSpace(playerNameInputField.text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public bool IsGenderChoosed
    {
        get
        {
            if(toggles[0].isOn || toggles[1].isOn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public string SetPlayerNameToButtonsName
    {
        get
        {
            return transform.name;
        }
        set
        {
            transform.name = value;
        }
    }
    public string GetPlayerSelectedGender { get; set; }


    void Update()
    {
        if (IsCanvasGroupActive)
        {
            GetComponent<Button>().interactable = IsNameSet && IsGenderChoosed;
            SetPlayerNameToButtonsName = playerNameInputField.text;

            ToggleActivty();
            GetPressedToggle();
        }
    }

    public void OnToggle(int index)
    {
        toggleEnum = (ToggleEnum)index;
        isTogglePressed = true;
    }

    void ToggleActivty()
    {
        if (toggleEnum == ToggleEnum.None)
        {
            toggles[0].isOn = false;
            toggles[1].isOn = false;
        }
        if (toggleEnum == ToggleEnum.ToggleMale && isTogglePressed)
        {
            toggles[0].isOn = true;
            toggles[1].isOn = false;

            isTogglePressed = false;
        }
        if (toggleEnum == ToggleEnum.ToggleFemale && isTogglePressed)
        {
            toggles[0].isOn = false;
            toggles[1].isOn = true;

            isTogglePressed = false;
        }
    }

    void GetPressedToggle()
    {
        if (toggles[0].isOn)
        {
            GetPlayerSelectedGender = PlayerKeys.Male;
        }
        if (toggles[1].isOn)
        {
            GetPlayerSelectedGender = PlayerKeys.Female;
        }
    }
}
