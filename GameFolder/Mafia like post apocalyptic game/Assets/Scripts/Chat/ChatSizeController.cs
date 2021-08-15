using UnityEngine;

public class ChatSizeController : MonoBehaviour
{
    [Header("RECT TRANSFORM")]
    [SerializeField] RectTransform chatRectTransform;

    public RectTransform ChatRectTransform => chatRectTransform;

    float defaultAnchorMaxY;
    float defaultOffsetMaxY;

    void Awake()
    {
        defaultAnchorMaxY = ChatRectTransform.anchorMax.y;
        defaultOffsetMaxY = ChatRectTransform.offsetMax.y;
    }

    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnChatResize += GlobalInputs_OnChatResize;
            PlayerBaseConditions._MyGameManager.OnDayVote += _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote += _MyGameManager_OnNightVote;
        });
    }
   
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameControllerComponentesNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnChatResize -= GlobalInputs_OnChatResize;
            PlayerBaseConditions._MyGameManager.OnDayVote -= _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote -= _MyGameManager_OnNightVote;
        }
    }

    #region Resize
    void Resize(bool maximize)
    {
        if (maximize)
        {
            ChatRectTransform.anchorMax = new Vector2(ChatRectTransform.anchorMax.x, 1);
            ChatRectTransform.offsetMax = new Vector2(ChatRectTransform.offsetMax.x, 0);
        }
        else
        {
            ChatRectTransform.anchorMax = new Vector2(ChatRectTransform.anchorMax.x, defaultAnchorMaxY);
            ChatRectTransform.offsetMax = new Vector2(ChatRectTransform.offsetMax.x, defaultOffsetMaxY);
        }

        PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(PlayerBaseConditions.Chat.ChatResizeSoundFX);
    }
    #endregion

    #region GlobalInputs_OnChatResize
    void GlobalInputs_OnChatResize(int obj)
    {
        if(obj == 0)
        {
            Resize(true);

            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, false);
        }
        else
        {
            Resize(false);

            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, true);
        }
    }
    #endregion

    #region _MyGameManager_OnNightVote
    void _MyGameManager_OnNightVote()
    {
        Resize(false);
    }
    #endregion

    #region _MyGameManager_OnDayVote
    void _MyGameManager_OnDayVote()
    {
        Resize(false);
    }
    #endregion

}
