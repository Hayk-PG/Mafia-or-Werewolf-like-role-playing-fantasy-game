using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInformation : MonoBehaviourPun
{
    public enum Gender { Male, Female }
    public Gender gender;

    public PhotonView PhotonView
    {
        get => GetComponent<PhotonView>();
    }
    public Player Player
    {
        get => PlayerBaseConditions.Player(PhotonView.OwnerActorNr);
    }
    public GameObject MyTagObject
    {
        get => (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(GetComponent<PhotonView>().OwnerActorNr).TagObject;
        set => PhotonNetwork.CurrentRoom.GetPlayer(GetComponent<PhotonView>().OwnerActorNr).TagObject = value;
    }
    public string GameObjectName
    {
        get => transform.name;
        set => transform.name = value;
    }

    void Start()
    {
        gender = Player.CustomProperties[PlayerKeys.GenderKey].ToString() == PlayerKeys.Female ? Gender.Female : Gender.Male;
        MyTagObject = gameObject;
        GameObjectName = Player.NickName;
    }
}
