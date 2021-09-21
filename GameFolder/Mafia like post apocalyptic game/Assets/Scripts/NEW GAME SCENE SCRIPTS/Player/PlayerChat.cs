using Photon.Pun;
using System;
using UnityEngine;

public class PlayerChat : MonoBehaviourPun
{
    static PlayerChat PC;

    ChatController _ChatController;


    void Awake()
    {
        Instance();
    }

    void Update()
    {
        OnClickChatButton();
    }

    #region Instance
    void Instance()
    {
        if (photonView.IsMine && photonView.AmOwner)
        {
            PC = this;
            _ChatController = FindObjectOfType<ChatController>();
        }
    }
    #endregion

    #region OnClickChatButton
    void OnClickChatButton()
    {
        if (_ChatController != null)
        {
            _ChatController._UI.SendButton.onClick.RemoveAllListeners();
            _ChatController._UI.SendButton.onClick.AddListener(() =>
            {
                if (!String.IsNullOrEmpty(_ChatController._UI.Text))
                {
                    PC.photonView.RPC("SendTextRPC", RpcTarget.All, PlayerBaseConditions.LocalPlayer.ActorNumber, _ChatController._UI.Text);
                    _ChatController._UI.Text = "";
                }
            });
        }
    }
    #endregion

    #region SendTextRPC
    [PunRPC]
    void SendTextRPC(int localActorNumber, string text)
    {
        string chatText = FindObjectOfType<ChatController>()._GameObjects.ChatContainer.childCount + ") " + "<b>" + "[" + PhotonNetwork.CurrentRoom.GetPlayer(localActorNumber).NickName + "] " + "</b>" + "<i>" + text + "</i>";
        FindObjectOfType<ChatController>().InstantiateChatText(chatText, new Color32(255, 255, 255, 255), new Color32(0, 0, 0, 50), 0);
    }
    #endregion
}
