using System;
using UnityEngine;
using UnityEngine.UI;

public class SignUpTab : MonoBehaviour
{
    public enum Gender { Male, Female, None}
    public Gender gender;

    [Header("INPUT FIELDS")]
    [SerializeField] protected InputField usernameInputField;
    [SerializeField] protected InputField passwordInputField;

    [Header("CANVAS GROUPS")]
    [SerializeField] protected CanvasGroup signUpCanvasGroup;
    [SerializeField] protected CanvasGroup signInCanvasGroup;
    [SerializeField] protected CanvasGroup signUpButtonCanvasGroup;
    [SerializeField] protected CanvasGroup signInButtonCanvasGroup;
    [SerializeField] protected CanvasGroup saveCanvasGroup;
    [SerializeField] protected CanvasGroup errorCanvasGroup;

    [Header("BUTTONS")]
    [SerializeField] protected Button[] genderButtons;
    [SerializeField] protected Button signUpButton;
    [SerializeField] protected Button signInButton;
    [SerializeField] protected Button saveButton;

    [Header("TEXTS")]
    [SerializeField] protected Text errorText;

    [Header("VARS")]
    [SerializeField] protected bool hasSaved;


    protected virtual void Update()
    {
        SignUpButtonCanvasGroupActivity();
        SaveCanvasGroupActivity(signUpButtonCanvasGroup);
        OnClickInputFields();
        OnClickGenderButtons();
        OnClickSaveButton();
        OnClickSignUpButton();
    }

    #region SignUpButtonCanvasGroupActivity
    void SignUpButtonCanvasGroupActivity()
    {
        MyCanvasGroups.CanvasGroupActivity(signUpButtonCanvasGroup, !String.IsNullOrEmpty(usernameInputField.text) && !String.IsNullOrEmpty(passwordInputField.text) && gender != Gender.None);
    }
    #endregion

    #region SignInButtonCanvasGroupActivity
    protected void SignInButtonCanvasGroupActivity()
    {
        MyCanvasGroups.CanvasGroupActivity(signInButtonCanvasGroup, !String.IsNullOrEmpty(usernameInputField.text) && !String.IsNullOrEmpty(passwordInputField.text));
    }
    #endregion

    #region SaveCanvasGroupActivity
    protected void SaveCanvasGroupActivity(CanvasGroup buttonCanvasGroup)
    {
        MyCanvasGroups.CanvasGroupActivity(saveCanvasGroup, buttonCanvasGroup.interactable);
    }
    #endregion

    #region OnClickInputFields
    protected void OnClickInputFields()
    {
        if (errorCanvasGroup.interactable)
        {
            if (usernameInputField.isFocused || passwordInputField.isFocused)
            {
                MyCanvasGroups.CanvasGroupActivity(errorCanvasGroup, false);
            }
        }
    }
    #endregion

    #region OnClickGenderButtons
    void OnClickGenderButtons()
    {
        for (int i = 0; i < genderButtons.Length; i++)
        {
            int index = i;
            genderButtons[index].onClick.RemoveAllListeners();
            genderButtons[index].onClick.AddListener(delegate
            {
                genderButtons[index].GetComponent<ButtonLocalSprite>().OnClickGenderButtons(true);
                genderButtons[index > 0 ? 0: 1].GetComponent<ButtonLocalSprite>().OnClickGenderButtons(false);

                gender = (Gender)index;
            });
        }
    }
    #endregion

    #region OnClickSaveButton
    protected void OnClickSaveButton()
    {
        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(delegate 
        {
            hasSaved = !hasSaved;

            saveButton.GetComponent<ButtonLocalSprite>().OnClickSwitchButton(hasSaved);
        });
    }
    #endregion

    #region OnClickSignUpButton
    void OnClickSignUpButton()
    {
        signUpButton.onClick.RemoveAllListeners();
        signUpButton.onClick.AddListener(delegate 
        {
            PlayerBaseConditions.PlayfabManager.PlayfabSignUp.OnPlayfabRegister(usernameInputField.text, passwordInputField.text, gender);

            if (hasSaved) PlayerBaseConditions.PlayerSavedData.SaveUsernameAndPassword(usernameInputField.text, passwordInputField.text);
            else PlayerBaseConditions.PlayerSavedData.DeleteUsernameAndPassword();
        });
    }
    #endregion

    #region OnClickSignInButton
    protected void OnClickSignInButton()
    {
        signInButton.onClick.RemoveAllListeners();
        signInButton.onClick.AddListener(delegate
        {
            PlayerBaseConditions.PlayfabManager.PlayfabSignIn.OnPlayfabLogin(usernameInputField.text, passwordInputField.text);

            if (hasSaved) PlayerBaseConditions.PlayerSavedData.SaveUsernameAndPassword(usernameInputField.text, passwordInputField.text);
            else PlayerBaseConditions.PlayerSavedData.DeleteUsernameAndPassword();
        });
    }
    #endregion

    #region OnPlayfabRegisterError
    public void OnPlayfabRegisterError(string errorMessage)
    {
        MyCanvasGroups.CanvasGroupActivity(signUpCanvasGroup, true);
        MyCanvasGroups.CanvasGroupActivity(errorCanvasGroup, true);

        errorText.text = errorMessage == "The display name entered is not available." ? "Sorry, that username already exists!" : errorMessage;

        usernameInputField.text = null;
        passwordInputField.text = null;

        gender = Gender.None;
        hasSaved = false;

        foreach (var genderButton in genderButtons)
        {
            genderButton.GetComponent<ButtonLocalSprite>().OnClickGenderButtons(hasSaved);
        }

        saveButton.GetComponent<ButtonLocalSprite>().OnClickSwitchButton(hasSaved);
    }
    #endregion



}
