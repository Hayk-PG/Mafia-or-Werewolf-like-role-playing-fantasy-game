using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviourPun
{
    [Header("CANVAS GROUP")]
    [SerializeField] CanvasGroup gameStartTab;
    [SerializeField] CanvasGroup gameStartButton;
    [SerializeField] CanvasGroup timerTab;
    [SerializeField] CanvasGroup abilityButtonsTab;

    [Header("SUN & MOON ICONS")]
    [SerializeField] Image sunIcon;
    [SerializeField] Image moonIcon;

    #region CanvasGroup
    public CanvasGroup GameStartTab => gameStartTab;
    public CanvasGroup GameStartButton => gameStartButton;
    public CanvasGroup TimerTab => timerTab;
    public CanvasGroup AbilityButtonsTab => abilityButtonsTab;
    #endregion
   
    Image SunIcon => sunIcon;
    Image MoonIcon => moonIcon;


    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate 
        {
            PlayerBaseConditions._MyGameManager.OnRoundChange += _MyGameManager_OnRoundChange;
        });
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnRoundChange -= _MyGameManager_OnRoundChange;
        }
    }

    #region CanvasGroupActivity
    public void CanvasGroupActivity(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
    #endregion

    /// <summary>
    /// Only master client can see GameStartTab
    /// </summary>
    /// <param name="actorNumber"></param>
    public void ShowGameStartButtonToMasterClient(int actorNumber)
    {
        if (PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).IsMasterClient)
        {
            CanvasGroupActivity(GameStartButton, true);
        }
    }

    void _MyGameManager_OnRoundChange()
    {
        if (PlayerBaseConditions.Night)
        {
            SunIcon.gameObject.SetActive(false);
            MoonIcon.gameObject.SetActive(true);
        }
        if (PlayerBaseConditions.Day)
        {
            SunIcon.gameObject.SetActive(true);
            MoonIcon.gameObject.SetActive(false);
        }
    }







}
