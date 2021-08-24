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
    
    public void ConnectionCheck(float waitForSeconds, Connected C, Action Connected, Action Reconnect)
    {
        StartCoroutine(CheckConnectionCoroutine(waitForSeconds, Connected, Reconnect));
    }

    IEnumerator CheckConnectionCoroutine(float waitForSeconds, Action Connected, Action Reconnect)
    {
        yield return new WaitForSeconds(waitForSeconds);

        if (IsConnected())
        {
            connectionScreen.SetActive(false);
            Connected?.Invoke();
        }
        else
        {
            connectionScreen.SetActive(true);
            Reconnect?.Invoke();
            yield return StartCoroutine(CheckConnectionCoroutine(waitForSeconds, Connected, Reconnect));
        }
    }

    bool IsConnected()
    {
        if (_Connected == Connected.IsConnected) return PhotonNetwork.IsConnected;
        if (_Connected == Connected.IsConnectedAndReady) return PhotonNetwork.IsConnectedAndReady;
        else return false;
    }
}
