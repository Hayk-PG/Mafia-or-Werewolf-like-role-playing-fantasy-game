using System;
using UnityEngine;
using UnityEngine.UI;

public class GlobalInputs : MonoBehaviour
{
    #region EVENTS
    public event Action<int> OnClickGameStartButton;
    public event Action<int> OnClickAvatarButtons;
    public event Action<int> OnChatResize;
    public event Action<InputField> OnChat;
    #endregion

    [Header("GAME START BUTTON")]
    [SerializeField] Button gameStartButton;

    [Header("AVATAR BUTTONS")]
    [SerializeField] Button[] avatarButtons;

    [Header("CHAT BUTTONS")]
    [SerializeField] Button[] chatButtons;

    /// <summary>
    /// Players related avatar buttons
    /// </summary>
    public Button[] AvatarButtons => avatarButtons;

    /// <summary>
    /// 0: Maximize 1: Minimize 2: Send
    /// </summary>
    public Button[] ChatButtons => chatButtons;


    void Update()
    {
        OngameStartButton();
        OnAvatarButtons();
        OnChatButtons();
    }

    #region OngameStartButton
    void OngameStartButton()
    {
        gameStartButton.onClick.RemoveAllListeners();
        gameStartButton.onClick.AddListener(() => { OnClickGameStartButton?.Invoke(Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber); });
    }
    #endregion

    #region OnAvatarButtons
    void OnAvatarButtons()
    {
        foreach (var button in AvatarButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClickAvatarButtons?.Invoke(int.TryParse(button.name, out int actorNumber) == true ? actorNumber: -1));
        }
    }
    #endregion

    #region OnChatButtons
    void OnChatButtons()
    {
        for (int i = 0; i < ChatButtons.Length; i++)
        {
            int index = i;

            ChatButtons[index].onClick.RemoveAllListeners();
            ChatButtons[index].onClick.AddListener(delegate 
            {
                if(index == 0 || index == 1)
                {
                    OnChatResize?.Invoke(index);
                }
                if (index == 2)
                {
                    OnChat?.Invoke(PlayerBaseConditions.Chat?.ChatInputField);
                }
            });
        }
    }
    #endregion

}
