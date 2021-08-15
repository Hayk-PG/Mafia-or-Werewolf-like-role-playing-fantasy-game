using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonsTabController : MonoBehaviour
{
    public event Action<int, int, string> OnClickAbilityButtons;

    [Header("CANVAS GROUP")]
    [SerializeField] CanvasGroup canvasGroup;

    [Header("BUTTONS")]
    [SerializeField] Button[] buttons;

    [Header("GIVEN ACTOR NUMBER")]
    [SerializeField] int givenActorNumber;

    [Header("VFX")]
    [SerializeField] GameObject vfx;

    public Button[] Buttons => buttons;

    public int GivenActorNumber
    {
        get
        {
            return givenActorNumber;
        }

        set
        {
            givenActorNumber = value;
        }
    }



    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate 
        {
            PlayerBaseConditions._MyGameManager.OnDay += _MyGameManager_OnDay;
        });
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnDay -= _MyGameManager_OnDay;
        }
    }

    void Update()
    {
        OnClickButtons();
    }

    #region CanvasGroupActivity
    void CanvasGroupActivity(bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
    #endregion

    #region OnClickToOpenAbilityButtonsTab
    public void OnClickToOpenAbilityButtonsTab(int actorNumber, string roleName)
    {
        GivenActorNumber = actorNumber;

        AssignButtonsText(roleName);
        CanvasGroupActivity(true);

        vfx.SetActive(true);
        PlayerBaseConditions.HideUnhideVFXByTags(Tags.NightTimeVFX, false);
    }
    #endregion

    #region OnClickToCloseAbilityButtonsTab
    public void OnClickToCloseAbilityButtonsTab()
    {
        vfx.SetActive(false);
        CanvasGroupActivity(false);
        PlayerBaseConditions.HideUnhideVFXByTags(Tags.NightTimeVFX, true);
    }
    #endregion

    #region AssignButtonsText
    void AssignButtonsText(string roleName)
    {
        if(roleName == RoleNames.Lizard)
        {
            if (Buttons[0].GetComponentInChildren<Text>().text != "Hide from the Sheriff")
            {
                Buttons[0].GetComponentInChildren<Text>().text = "Hide from the Sheriff";
            }
            if (Buttons[1].GetComponentInChildren<Text>().text != "Unreadable names")
            {
                Buttons[1].GetComponentInChildren<Text>().text = "Unreadable names";
            }
            if (Buttons[2].GetComponentInChildren<Text>().text != "Confuse")
            {
                Buttons[2].GetComponentInChildren<Text>().text = "Confuse";
            }
        }
    }
    #endregion

    #region _MyGameManager_OnDay
    void _MyGameManager_OnDay()
    {
        if (PlayerBaseConditions._IsMyGameControllerComponentesNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.GameUI.CanvasGroupActivity(PlayerBaseConditions._MyGameControllerComponents.GameUI.AbilityButtonsTab, false);
        }
    }
    #endregion

    #region OnClickButtons
    void OnClickButtons()
    {
        for (int b = 0; b < Buttons.Length; b++)
        {
            int index = b;

            Buttons[index].onClick.RemoveAllListeners();
            Buttons[index].onClick.AddListener(() => OnClickAbilityButtons?.Invoke(index, GivenActorNumber, PlayerBaseConditions._LocalPlayerTagObject.GetComponent<ISetPlayerRoleProps>().RoleName));
        }
    }
    #endregion















}
