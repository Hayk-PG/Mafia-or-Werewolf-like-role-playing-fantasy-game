using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatSizeController : MonoBehaviour
{
    [Serializable] class Conditions
    {
        [SerializeField] CanvasGroup chatCanvasGroup;

        internal bool CanUseTheChat
        {
            get => chatCanvasGroup.interactable;
            set => chatCanvasGroup.interactable = value;
        }
    }
    [Serializable] class ResizeButtons
    {
        [SerializeField] internal Button maximize;
        [SerializeField] internal Button minimize;
    }

    [SerializeField] Conditions _Conditions;  
    [SerializeField] ResizeButtons _ResizeButtons;

    [Header("RECT TRANSFORM")]
    [SerializeField] RectTransform chatRectTransform;

    public RectTransform ChatRectTransform => chatRectTransform;

    float defaultAnchorMaxY;
    float defaultOffsetMaxY;

    GameManagerTimer _GameManagerTimer;

    void Awake()
    {
        defaultAnchorMaxY = ChatRectTransform.anchorMax.y;
        defaultOffsetMaxY = ChatRectTransform.offsetMax.y;

        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
    }

    void Start()
    {
        _Conditions.CanUseTheChat = true;
    }

    void OnEnable()
    {
        _GameManagerTimer.IsResetPhaseActive += CanUseTheChat;
    }

    void OnDisable()
    {
        _GameManagerTimer.IsResetPhaseActive -= CanUseTheChat;
    }

    void Update()
    {
        _ResizeButtons.maximize.onClick.RemoveAllListeners();
        _ResizeButtons.maximize.onClick.AddListener(() => { Resize(true); });

        _ResizeButtons.minimize.onClick.RemoveAllListeners();
        _ResizeButtons.minimize.onClick.AddListener(() => { Resize(false); });
    }

    #region CanUseTheChat
    void CanUseTheChat(bool canUseTheChat)
    {
        _Conditions.CanUseTheChat = canUseTheChat;

        if (!canUseTheChat) Resize(false);
    }
    #endregion

    #region Resize
    void Resize(bool maximize)
    {
        if (maximize)
        {
            ChatRectTransform.anchorMax = new Vector2(ChatRectTransform.anchorMax.x, 1);
            ChatRectTransform.offsetMax = new Vector2(ChatRectTransform.offsetMax.x, 0);
            PlayerBaseConditions.VFXCamera().enabled = false;
        }
        else
        {
            ChatRectTransform.anchorMax = new Vector2(ChatRectTransform.anchorMax.x, defaultAnchorMaxY);
            ChatRectTransform.offsetMax = new Vector2(ChatRectTransform.offsetMax.x, defaultOffsetMaxY);
            PlayerBaseConditions.VFXCamera().enabled = true;
        }
    }
    #endregion
}
