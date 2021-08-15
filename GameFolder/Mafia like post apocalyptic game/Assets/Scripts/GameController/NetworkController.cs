using Photon.Pun;
using UnityEngine;

public class NetworkController : MonoBehaviourPun
{

    void Start()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnClickGameStartButton += GlobalInputs_OnClickGameStartButton;
        });
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameControllerComponentesNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnClickGameStartButton -= GlobalInputs_OnClickGameStartButton;
        }
    }

    void GlobalInputs_OnClickGameStartButton(int obj)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }











}
