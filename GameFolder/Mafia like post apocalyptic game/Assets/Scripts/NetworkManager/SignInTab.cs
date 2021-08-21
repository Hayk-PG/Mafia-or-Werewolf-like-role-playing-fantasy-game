using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInTab : SignUpTab
{


    protected override void Update()
    {
        SignInButtonCanvasGroupActivity();
        SaveCanvasGroupActivity(signInButtonCanvasGroup);
        OnClickInputFields();
        OnClickSaveButton();
        OnClickSignInButton();
        OnClickBackButton();
    }

    #region OnPlayfabRegisterError
    public new void OnPlayfabRegisterError(string errorMessage)
    {
        MyCanvasGroups.CanvasGroupActivity(signInCanvasGroup, true);
        MyCanvasGroups.CanvasGroupActivity(errorCanvasGroup, true);

        errorText.text = errorMessage == "The display name entered is not available." ? "Sorry, that username already exists!" : errorMessage;

        usernameInputField.text = null;
        passwordInputField.text = null;

        hasSaved = false;

        saveButton.GetComponent<ButtonLocalSprite>().OnClickSwitchButton(hasSaved);
    }
    #endregion
}
