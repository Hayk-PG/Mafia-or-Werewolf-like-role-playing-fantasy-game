using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviourPun
{
    public static Options instance;

    [Serializable] public class Buttons
    {
        [SerializeField] GameObject[] buttonsObj;

        public GameObject[] ButtonsObj
        {
            get => buttonsObj;
        }
    }
    [Serializable] public class OptionsUI
    {
        [Header("CANVAS GROUP")]
        [SerializeField] CanvasGroup optionsTab;
        [SerializeField] CanvasGroup optionsButtonTab;
        [SerializeField] CanvasGroup exitTab;

        public CanvasGroup OptionsTab => optionsTab;
        public CanvasGroup OptionsButtonTab => optionsButtonTab;
        public CanvasGroup ExitTab => exitTab;
    }

    public Buttons _Buttons;
    public OptionsUI _OptionsUI;

    public PlayerBadgeButton _PlayerBadgeButton;

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

    void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    #region SceneManager_activeSceneChanged
    void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.name == SceneNames.MenuScene)
        {
            OnMenuScene();
        }
    }
    #endregion

    #region OnMenuScene
    public void OnMenuScene()
    {
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, true);
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);
        ButtonsActivity(0, true);
        ButtonsActivity(1, true);
        ButtonsActivity(2, false);
        ButtonsActivity(3, false);
        ButtonsActivity(4, true);
        ButtonsActivity(5, true);
    }
    #endregion

    #region OnPressedOptionButton
    public void OnPressedOptionButton()
    {
        OnSceneChanges(IsDone =>
        {
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, true);
            MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);
        });
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

    #region OnSceneChanges
    void OnSceneChanges(Action<bool> IsDone)
    {
        bool isDOne = false;

        if (_MySceneManager.CurrentScene().name == SceneNames.MenuScene)
        {
            ButtonsActivity(0, true);
            ButtonsActivity(1, true);
            ButtonsActivity(2, false);
            ButtonsActivity(3, false);
            ButtonsActivity(4, true);
            ButtonsActivity(5, true);
            isDOne = true;
        }
        if (_MySceneManager.CurrentScene().name == SceneNames.MultiplayerScene)
        {
            ButtonsActivity(0, false);
            ButtonsActivity(1, false);
            ButtonsActivity(2, true);
            ButtonsActivity(3, true);
            ButtonsActivity(4, true);
            ButtonsActivity(5, false);
            isDOne = true;
        }
        if (_MySceneManager.CurrentScene().name == SceneNames.SinglePlayerScene)
        {
            ButtonsActivity(0, false);
            ButtonsActivity(1, false);
            ButtonsActivity(2, true);
            ButtonsActivity(3, true);
            ButtonsActivity(4, true);
            ButtonsActivity(5, false);
            isDOne = true;
        }

        IsDone?.Invoke(isDOne);
    }
    #endregion

    /// <summary>
    /// 0: SInglePlayer 1: Multiplayer 2: Resume 3: MainMenu 4: Exit 5: PlayerBadge
    /// </summary>
    /// <param name="index"></param>
    /// <param name="enabled"></param>
    public void ButtonsActivity(int index, bool enabled)
    {
        if (_Buttons.ButtonsObj[index].activeInHierarchy != enabled) _Buttons.ButtonsObj[index].SetActive(enabled);
    }

}
