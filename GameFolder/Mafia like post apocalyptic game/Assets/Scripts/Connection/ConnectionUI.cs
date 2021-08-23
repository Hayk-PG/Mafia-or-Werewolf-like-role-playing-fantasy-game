using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectionUI : MonoBehaviourPun
{
    public static ConnectionUI instance;

    public enum Connected { IsConnected, IsConnectedAndReady}
    public Connected _Connected;


    [SerializeField] GameObject connectionScreen;
    [SerializeField] float checkTime;


    void Awake()
    {
        CreateAsInstance();
    }

    #region CreateAsInstance
    void CreateAsInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    public void ConnectionCheck(Connected C, Action Connected, Action Reconnect)
    {
        StartCoroutine(CheckConnectionCoroutine(Connected, Reconnect));
    }

    IEnumerator CheckConnectionCoroutine(Action Connected, Action Reconnect)
    {
        yield return new WaitForSeconds(checkTime);

        if (IsConnected())
        {
            connectionScreen.SetActive(false);
            Connected?.Invoke();
        }
        else
        {
            connectionScreen.SetActive(true);
            Reconnect?.Invoke();
            yield return StartCoroutine(CheckConnectionCoroutine(Connected, Reconnect));
        }
    }

    bool IsConnected()
    {
        if (_Connected == Connected.IsConnected) return PhotonNetwork.IsConnected;
        if (_Connected == Connected.IsConnectedAndReady) return PhotonNetwork.IsConnectedAndReady;
        else return false;
    }
}
