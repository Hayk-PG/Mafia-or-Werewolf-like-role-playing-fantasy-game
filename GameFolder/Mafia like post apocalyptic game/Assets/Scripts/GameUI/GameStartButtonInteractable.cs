using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButtonInteractable : MonoBehaviourPun
{
    [SerializeField] CanvasGroup gameStartTab;

    bool IsGameStartTabOpen
    {
        get
        {
            return gameStartTab.interactable;
        }
    }
    bool ButtonInteractable
    {
        get
        {
            return GetComponent<Button>().interactable;
        }
        set
        {
            GetComponent<Button>().interactable = value;
        }
    }


    void Update()
    {
        if (IsGameStartTabOpen)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(RoomCustomProperties.MinRequiredCount))
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= (int)PhotonNetwork.CurrentRoom.CustomProperties[RoomCustomProperties.MinRequiredCount])
                {
                    if (ButtonInteractable == false)
                    {
                        ButtonInteractable = true;
                    }
                }
                else
                {
                    if (ButtonInteractable == true)
                    {
                        ButtonInteractable = false;
                    }
                }
            }
        }
    }

}
