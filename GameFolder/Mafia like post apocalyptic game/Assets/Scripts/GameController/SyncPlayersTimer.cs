using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayersTimer : MonoBehaviourPun
{
    public static SyncPlayersTimer GSync;

    [Header("REAL TIME")]
    int mostSyncedPlayerActrNmbr;
    public PlayerSelfTimer MostSyncedPlayerTimer { get; set; }


    void Awake()
    {
        GSync = this;
    }

    void Start()
    {
        GSync.StartCoroutine(Sync());
    }

    IEnumerator Sync()
    {
        List<byte> seconds = new List<byte>();
        List<byte> nightsCount = new List<byte>();
        List<byte> daysCount = new List<byte>();

        yield return new WaitForSeconds(1);

        GSync.CachePlayersData(seconds, nightsCount, daysCount);

        yield return null;

        seconds.Sort();
        nightsCount.Sort();
        daysCount.Sort();

        yield return null;

        GetMostSyncedPlayerActrNmbr(seconds, nightsCount, daysCount);

        yield return null;

        GSync.photonView.RPC("GetMostCorrectTime", RpcTarget.All, GSync.mostSyncedPlayerActrNmbr);

        yield return StartCoroutine(Sync());
    }

    void CachePlayersData(List<byte> seconds, List<byte> nightsCount, List<byte> daysCount)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            if (playerObj != null)
            {
                seconds.Add(playerObj.GetComponent<PlayerSelfTimer>().Second);
                nightsCount.Add(playerObj.GetComponent<PlayerSelfTimer>().NightsCount);
                daysCount.Add(playerObj.GetComponent<PlayerSelfTimer>().DaysCount);
            }
        }
    }

    void GetMostSyncedPlayerActrNmbr(List<byte> seconds, List<byte> nightsCount, List<byte> daysCount)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            if (playerObj != null)
            {
                if (playerObj.GetComponent<PlayerSelfTimer>().Second >= seconds[seconds.Count - 1] && playerObj.GetComponent<PlayerSelfTimer>().NightsCount >= nightsCount[nightsCount.Count - 1] && playerObj.GetComponent<PlayerSelfTimer>().DaysCount >= daysCount[daysCount.Count - 1])
                {
                    GSync.mostSyncedPlayerActrNmbr = playerObj.GetComponent<SetPlayerInfo>().ActorNumber;
                }
            }
        }
    }



}
