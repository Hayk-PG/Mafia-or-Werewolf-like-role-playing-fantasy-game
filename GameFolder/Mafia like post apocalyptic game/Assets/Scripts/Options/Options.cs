using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviourPun
{
    public static Options instance;

    public OptionsUI _OptionsUI;
    [SerializeField] ButtonsInteractability _ButtonsInteractability;

    #region OptionsUI
    [Serializable]
    public class OptionsUI
    {
        [Header("CANVAS GROUP")]
        [SerializeField] CanvasGroup optionsTab;
        [SerializeField] CanvasGroup optionsButtonTab;
        [SerializeField] CanvasGroup exitTab;

        public CanvasGroup OptionsTab => optionsTab;
        public CanvasGroup OptionsButtonTab => optionsButtonTab;
        public CanvasGroup ExitTab => exitTab;
    }
    #endregion

    #region ButtonsInteractability
    [Serializable] [SerializeField] class ButtonsInteractability
    {
        [SerializeField] internal Button[] Buttons;
    }
    #endregion

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        OnButtonsInteractability();
    }

    #region OnPressedOptionButton
    public void OnPressedOptionButton()
    {
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, true);
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);        
    }
    #endregion

    #region OnPressedOptionsButtons
    public void OnPressedOptionsButtons()
    {
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, false);
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, true);        
    }
    #endregion

    #region OnPressedExitButton
    public void OnPressedExitButton(bool isTabOpened)
    {
        if (isTabOpened)
        {
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, false);
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.ExitTab, true);
        }
        else
        {
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.ExitTab, false);
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, true);            
        }
    }
    #endregion

    #region OnButtonsInteractability
    void OnButtonsInteractability()
    {
        if(_MySceneManager.CurrentScene().name == SceneNames.MenuScene)
        {
            if (_ButtonsInteractability.Buttons[0].interactable)
            {
                foreach (var button in _ButtonsInteractability.Buttons)
                {
                    button.interactable = false;
                }
            }
        }
        else
        {
            if (!_ButtonsInteractability.Buttons[0].interactable)
            {
                foreach (var button in _ButtonsInteractability.Buttons)
                {
                    button.interactable = true;
                }
            }
        }
    }
    #endregion
}
