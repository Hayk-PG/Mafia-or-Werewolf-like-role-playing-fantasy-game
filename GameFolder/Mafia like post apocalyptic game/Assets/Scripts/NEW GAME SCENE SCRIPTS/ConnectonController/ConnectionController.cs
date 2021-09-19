using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ConnectionController : MonoBehaviourPun
{
    public bool Disconnect;
    public bool Connected
    {
        get
        {
            if (_MySceneManager.CurrentScene().name == SceneNames.GameScene) return PhotonNetwork.InRoom;
            else return PhotonNetwork.IsConnected;
        }
    }

    delegate bool Connect();
    Connect _Reconnect;


    void Update()
    {
        if (_MySceneManager.CurrentScene().name == SceneNames.GameScene) _Reconnect = PhotonNetwork.ReconnectAndRejoin; else _Reconnect = PhotonNetwork.Reconnect;

        if (Disconnect && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Disconnect = false;
        }
    }

    public void Reconnect()
    {
        StartCoroutine(ReconnectCoroutine());
    }

    IEnumerator ReconnectCoroutine()
    {
        int attempts = 0;

        while (attempts < 5 && !Connected)
        {
            _Reconnect();

            attempts++;

            print(PhotonNetwork.NetworkClientState);

            yield return new WaitForSeconds(5);           
        }

        yield return null;
    }
}
