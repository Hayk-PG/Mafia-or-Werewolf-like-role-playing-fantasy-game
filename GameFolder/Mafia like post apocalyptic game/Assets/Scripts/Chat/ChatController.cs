using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviourPun
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

    NetworkCallbacks _NetworkCallbacks { get; set; }


    void Awake()
    {
        chat = this;
        _NetworkCallbacks = FindObjectOfType<NetworkCallbacks>();
    }

    void OnEnable()
    {
        _NetworkCallbacks.UpdateChatMessage += UpdateChatCallback;
    }

    void OnDisable()
    {
        _NetworkCallbacks.UpdateChatMessage -= UpdateChatCallback;
    }

    #region UpdateChatCallback
    void UpdateChatCallback(string playerName, bool hasNewPlayerJoined)
    {
        StartCoroutine(GetPhotonview(photonview => 
        {
            if (photonview.IsMine)
            {
                string messageText = playerName == null ? "Welcome " + PhotonNetwork.NickName + "!" : playerName != null && hasNewPlayerJoined ? playerName + " joined the game!" : playerName != null && !hasNewPlayerJoined ? playerName + " left the game!" : null;

                InstantiateChatText(messageText, new Color32(242, 255, 0, 255), new Color32(255, 19, 0, 50), 1);
            }
        }));   
    }
    #endregion

    #region InstantiateChatText
    public void InstantiateChatText(string text, Color textColor, Color backgroundColor, int soundFXIndex)
    {
        Text chatText = Instantiate(TextPrefab, ChatContainer);
        chatText.color = textColor;
        chatText.GetComponentInChildren<Image>().color = backgroundColor;
        chatText.text = text;
        //PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(soundFXIndex == 0 ? ChatMessageSoundFX[0]: ChatMessageSoundFX[1]);
    }
    #endregion

    IEnumerator GetPhotonview(Action<PhotonView> photonview)
    {
        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.TagObject != null);
        GameObject localPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
        photonview?.Invoke(localPlayer.GetComponent<PhotonView>());
    }
}
