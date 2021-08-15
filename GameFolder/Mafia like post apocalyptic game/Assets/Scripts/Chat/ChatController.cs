using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour
{
    public static ChatController chat;

    [Header("INPUT FIELD")]
    [SerializeField] InputField chatInputField;

    [Header("TRANSFORM")]
    [SerializeField] Transform chatContainer;

    [Header("PREFAB")]
    [SerializeField] Text textPrefab;

    [Header("SOUND")]
    [SerializeField] AudioClip[] chatMessageSoundFX;
    [SerializeField] AudioClip chatResizeSoundFX;

    public InputField ChatInputField => chatInputField;
    public Transform ChatContainer => chatContainer;
    public Text TextPrefab => textPrefab;

    /// <summary>
    /// 0: Player message 1: AI message
    /// </summary>
    public AudioClip[] ChatMessageSoundFX => chatMessageSoundFX;
    public AudioClip ChatResizeSoundFX => chatResizeSoundFX;



    void Awake()
    {
        chat = this;
    }

    public void InstantiateChatText(string text, Color textColor, Color backgroundColor, int soundFXIndex)
    {
        Text chatText = Instantiate(TextPrefab, ChatContainer);
        chatText.color = textColor;
        chatText.GetComponentInChildren<Image>().color = backgroundColor;
        chatText.text = text;
        PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(soundFXIndex == 0 ? ChatMessageSoundFX[0]: ChatMessageSoundFX[1]);
    }
   
}
