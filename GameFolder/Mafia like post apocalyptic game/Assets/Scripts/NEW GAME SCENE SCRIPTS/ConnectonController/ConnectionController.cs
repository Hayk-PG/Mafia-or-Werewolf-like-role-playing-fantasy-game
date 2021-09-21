using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ConnectionController : MonoBehaviourPun
{
    public bool disconnect;
    public bool reconnect;
    public bool Connected
    {
        get
        {
            if (_MySceneManager.CurrentScene().name == SceneNames.GameScene) return PhotonNetwork.InRoom;
            else return PhotonNetwork.IsConnected;
        }
    }
    public int connectingAttempts;

    delegate bool Connect();
    Connect _Reconnect;


    void Update()
    {
        if (_MySceneManager.CurrentScene().name == SceneNames.GameScene) _Reconnect = PhotonNetwork.ReconnectAndRejoin; else _Reconnect = PhotonNetwork.Reconnect;

        if (disconnect && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            reconnect = false;
        }
        if(reconnect && !PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Reconnect();
            disconnect = false;
        }
    }

    public void Reconnect()
    {
        StartCoroutine(ReconnectCoroutine());
    }

    IEnumerator ReconnectCoroutine()
    {
        int attempts = 0;

        yield return null;

        while (attempts < connectingAttempts && !Connected)
        {
            _Reconnect();

            attempts++;

            print(PhotonNetwork.NetworkClientState);

            yield return new WaitForSeconds(5);           
        }
     
        if (!Connected)
        {
            if (_MySceneManager.CurrentScene().name != SceneNames.MenuScene)
            {
                _MySceneManager.ChangeToMenuScene();
                StartCoroutine(ReconnectCoroutine());
            }
            else
            {
                StartCoroutine(ReconnectCoroutine());                
            }
        }
    }
}
