using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviourPun
{
    [Serializable] public class UI
    {
        [SerializeField] InputField inputField;
        [SerializeField] Button button;

        public string Text
        {
            get => inputField.text;
            set => inputField.text = value;
        }
        public Button SendButton
        {
            get => button;
        }
    }
    [Serializable] public class GameObjects
    {
        [SerializeField] Transform chatContainer;
        [SerializeField] Text textPrefab;

        public Transform ChatContainer
        {
            get => chatContainer;
        }
        public Text TextPrefab
        {
            get => textPrefab;
        }

    }

    public UI _UI;
    public GameObjects _GameObjects;
    NetworkCallbacks _NetworkCallbacks;

    void Awake()
    {
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
            if (photonview.IsMine && photonview.AmOwner)
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
        Text chatText = Instantiate(_GameObjects.TextPrefab, _GameObjects.ChatContainer);
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
