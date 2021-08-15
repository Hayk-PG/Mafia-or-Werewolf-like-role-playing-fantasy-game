using Photon.Pun;

public class NetworkManagerLoadLevel : MonoBehaviourPun
{
    void OnEnable()
    {
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomJoined += NetworkManager_OnRoomJoined;
    }
    
    void OnDisable()
    {
        GetComponent<NetworkManagerComponents>().NetworkManager.OnRoomJoined -= NetworkManager_OnRoomJoined;
    }

    void NetworkManager_OnRoomJoined()
    {
        PhotonNetwork.LoadLevel(SceneNames.GameScene);
    }







}
