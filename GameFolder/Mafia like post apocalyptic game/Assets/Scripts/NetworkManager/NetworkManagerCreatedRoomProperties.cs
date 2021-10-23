using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerCreatedRoomProperties : MonoBehaviour
{
    public event Action<string, bool, string, int> OnClickConfirmRoomButton;

    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup passwordImputFieldCanvasGroup;
    [SerializeField] CanvasGroup confirmRoomPropertiesButtonCanvasGroup;

    [Header("INPUT FIELDS")]
    [SerializeField] InputField roomNameInputField;
    [SerializeField] InputField passwordInputField;

    [Header("BUTTON")]
    [SerializeField] Button passwordSwitchButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button confirmRoomPropertiesButton;

    [Header("SLIDER")]
    [SerializeField] Slider minRequiredCountSlider;

    [SerializeField] Text minRequiredCountText;

    [Header("IMAGES")]
    [SerializeField] Image buttonImage;
    [SerializeField] Image buttonChildImage;

    [Header("SPRITES")]
    [SerializeField] Sprite switchOn;
    [SerializeField] Sprite switchOff;

    [SerializeField] bool isPasswordOn = false;

    public string RoomName
    {
        get
        {
            return roomNameInputField.text;
        }
    }
    public string PinNumber
    {
        get
        {
            return passwordInputField.text;
        }
    }
    public int MinRequiredCount
    {
        get
        {
            return (int)minRequiredCountSlider.value;
        }
    }
    public bool IsPasswordSet
    {
        get
        {
            return !String.IsNullOrEmpty(passwordInputField.text);
        }
    }
    
    
    void Update()
    {
        continueButton.interactable = isPasswordOn && IsPasswordSet || !isPasswordOn && !IsPasswordSet ? true : false;

        OnClickButton();
        OnClickConfirmRoomPropertiesButton();
    }
   
    #region OnClickButton
    void OnClickButton()
    {
        passwordSwitchButton.onClick.RemoveAllListeners();
        passwordSwitchButton.onClick.AddListener(delegate 
        {
            PasswordSwitch(passwordSwitchButton);
        });
    }
    #endregion

    #region PasswordSwitch
    void PasswordSwitch(Button button)
    {
        isPasswordOn = !isPasswordOn;

        if (isPasswordOn)
        {
            buttonImage.color = new Color32(11, 212, 9, 255);
            buttonChildImage.sprite = switchOn;
            passwordInputField.interactable = true;
            //MyCanvasGroups.CanvasGroupActivity(passwordImputFieldCanvasGroup, true);
        }
        else
        {
            buttonImage.color = new Color32(212, 29, 9, 255);
            buttonChildImage.sprite = switchOff;
            //MyCanvasGroups.CanvasGroupActivity(passwordImputFieldCanvasGroup, false);
            passwordInputField.interactable = false;
            passwordInputField.text = null;
        }
    }
    #endregion

    #region OnMinRequiredCountSliderValueChange
    public void OnMinRequiredCountSliderValueChange(Slider slider)
    {
        minRequiredCountText.text = slider.value.ToString();
    }
    #endregion

    #region OnClickConfirmRoomPropertiesButton
    void OnClickConfirmRoomPropertiesButton()
    {
        confirmRoomPropertiesButton.onClick.RemoveAllListeners();
        confirmRoomPropertiesButton.onClick.AddListener(delegate
        {
            OnClickConfirmRoomButton?.Invoke(RoomName, IsPasswordSet, PinNumber, MinRequiredCount);
        });
    }
    #endregion


}
