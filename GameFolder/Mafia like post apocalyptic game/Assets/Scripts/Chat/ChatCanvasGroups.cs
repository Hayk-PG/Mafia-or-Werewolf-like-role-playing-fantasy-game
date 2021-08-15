using UnityEngine;

public class ChatCanvasGroups : MonoBehaviour
{
    [Header("CANVAS GROUPS")]
    [SerializeField] CanvasGroup[] canvasGroups;

    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameManager.OnDay += _MyGameManager_OnDay;
            PlayerBaseConditions._MyGameManager.OnNight += _MyGameManager_OnNight;
            PlayerBaseConditions._MyGameManager.OnDayVote += _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote += _MyGameManager_OnNightVote;
        });
    }

    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameControllerComponentesNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnDay -= _MyGameManager_OnDay;
            PlayerBaseConditions._MyGameManager.OnNight -= _MyGameManager_OnNight;
            PlayerBaseConditions._MyGameManager.OnDayVote -= _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote -= _MyGameManager_OnNightVote;
        }
    }

    #region CanvasGroupsActivity
    void CanvasGroupsActivity(bool isInteractable)
    {
        foreach (var canvasGroup in canvasGroups)
        {
            canvasGroup.interactable = isInteractable;
        }
    }
    #endregion

    #region _MyGameManager_OnNight
    void _MyGameManager_OnNight()
    {
        CanvasGroupsActivity(true);
    }
    #endregion

    #region _MyGameManager_OnDay
    void _MyGameManager_OnDay()
    {
        CanvasGroupsActivity(true);
    }
    #endregion

    #region _MyGameManager_OnNightVote
    void _MyGameManager_OnNightVote()
    {
        CanvasGroupsActivity(false);
    }
    #endregion

    #region _MyGameManager_OnDayVote
    void _MyGameManager_OnDayVote()
    {
        CanvasGroupsActivity(false);
    }
    #endregion
}
