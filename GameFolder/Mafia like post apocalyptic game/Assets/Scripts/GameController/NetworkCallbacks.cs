using Photon.Pun;
using Photon.Realtime;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    public delegate void Callback();
    public delegate void ChatMessageCallback(string playerName, bool hasNewPlayerJoinded);
    public Callback UpdateOnlinePlayersListCallback;
    public ChatMessageCallback UpdateChatMessage;


    void Start()
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(null, false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(newPlayer.NickName, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(otherPlayer.NickName, false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        
    }

    public override void OnJoinedRoom()
    {
        
    }
}
