using Photon.Pun;
using UnityEngine;

public class InstantiatePhotonPlayers : MonoBehaviourPun
{
    void Start()
    {
        PlayerInstantiation();
    }

    void PlayerInstantiation()
    {
        GameObject player = PhotonNetwork.Instantiate(PlayerKeys.PlayerPrefabName.PlayerPrefab, Vector3.zero, Quaternion.identity);
    }
}
