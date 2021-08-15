using Photon.Pun;
using UnityEngine;

public class SetPlayerInfo : MonoBehaviourPun
{   
    #region GENDER
    public enum PlayerGender { Male, Female}
    public PlayerGender playerGender;
    #endregion

    public GameObject PlayerTagObject
    {
        get
        {
            return (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(GetComponent<PhotonView>().OwnerActorNr).TagObject;
        }
        set
        {
            PhotonNetwork.CurrentRoom.GetPlayer(GetComponent<PhotonView>().OwnerActorNr).TagObject = value;
        }
    }
    public PhotonView PlayerPhotonView
    {
        get
        {
            return GetComponent<PhotonView>();
        }
    }
    public int ViewID
    {
        get
        {
            return GetComponent<PhotonView>().ViewID;
        }
    }
    public int ActorNumber
    {
        get
        {
            return GetComponent<PhotonView>().OwnerActorNr;
        }
    }
    public string Name
    {
        get
        {
            return PhotonNetwork.CurrentRoom.GetPlayer(ActorNumber).NickName;
        }
    }
    public string SetGameObjectName
    {
        get
        {
            return transform.name;
        }
        set
        {
            transform.name = value;
        }
    }


    void Start()
    {       
        SetGameObjectsName();
        SetPlayerGender();
        SetTagObject();
    }

    void SetGameObjectsName()
    {
        SetGameObjectName = Name;
    }

    void SetPlayerGender()
    {
        playerGender = (string)PhotonNetwork.CurrentRoom.GetPlayer(ActorNumber).CustomProperties[PlayerKeys.GenderKey] == PlayerKeys.Male ? PlayerGender.Male :
            PlayerGender.Female;
    }

    void SetTagObject()
    {
        PlayerTagObject = gameObject;
    }























}
