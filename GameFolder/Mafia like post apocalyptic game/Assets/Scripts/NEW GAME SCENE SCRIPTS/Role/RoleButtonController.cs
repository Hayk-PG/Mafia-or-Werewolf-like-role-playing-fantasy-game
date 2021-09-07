﻿using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoleButtonController : MonoBehaviourPun
{   
    [Serializable] public struct OwnerInfo
    {
        [SerializeField] string ownerName;
        [SerializeField] string ownerUserId;
        [SerializeField] int ownerActorNumber;
        [SerializeField] GameObject owenrObj;

        public string OwnerName
        {
            get => ownerName;
            set => ownerName = value;
        }
        public string OwenrUserId
        {
            get => ownerUserId;
            set => ownerUserId = value;
        }
        public int OwnerActorNumber
        {
            get => ownerActorNumber;
            set => ownerActorNumber = value;
        }
        public GameObject OwnerObj
        {
            get => owenrObj;
            set => owenrObj = value;
        }
    }
    [Serializable] public struct UI
    {
        [SerializeField] Text name;
        [SerializeField] Text votesCount;
        [SerializeField] Text voteName;
        [SerializeField] Sprite roleSprite;
        [SerializeField] Image visibleToEveryoneImage;
        [SerializeField] CanvasGroup votesCountTextCanvasGroup;

        public string Name
        {
            get => name.text;
            set => name.text = value;
        }
        public string VoteName
        {
            get => voteName.text;
            set => voteName.text = value;
        }
        public int VotesCount
        {
            get => int.Parse(votesCount.text);
            set => votesCount.text = value.ToString();
        }
        public Sprite RoleImage
        {
            get => roleSprite;
            set => roleSprite = value;
        }
        public Sprite VisibleToEveryoneImage
        {
            get => visibleToEveryoneImage.sprite;
            set => visibleToEveryoneImage.sprite = value;
        }
        public CanvasGroup VotesCountTextCanvasGroup
        {
            get => votesCountTextCanvasGroup;
        }
    }
    [Serializable] public struct GameInfo
    {
        [SerializeField] string roleName;
        [SerializeField] int roleIndex;
        [SerializeField] bool isPlayerAlive;
        [SerializeField] bool isPlayerHealed;

        public string RoleName
        {
            get => roleName;
            set => roleName = value;
        }
        public int RoleIndex
        {
            get => roleIndex;
            set => roleIndex = value;
        }
        public bool IsPlayerAlive
        {
            get => isPlayerAlive;
            set => isPlayerAlive = value;
        }
        public bool IsPlayerHealed
        {
            get => isPlayerHealed;
            set => isPlayerHealed = value;
        }
    }
    [Serializable] public struct GameObjects
    {
        [SerializeField] internal GameObject votedNameIconObj;

        [SerializeField] GameObject voteFX;
        [SerializeField] GameObject voteFxExplosion;

        [SerializeField] GameObject diedIcon;
        [SerializeField] GameObject diedIcon2;
        [SerializeField] GameObject diedGoreExplosion;

        public GameObject VoteFX
        {
            get => voteFX;
        }
        public GameObject VoteFxExplosion
        {
            get => voteFxExplosion;
        }
        public GameObject DiedIcon
        {
            get => diedIcon;
        }
        public GameObject DiedIcon2
        {
            get => diedIcon2;
        }
        public GameObject DiedGoreExplosion
        {
            get => diedGoreExplosion;
        }
    }
    
    public OwnerInfo _OwnerInfo;
    public UI _UI;
    public GameInfo _GameInfo;
    public GameObjects _GameObjects;

    public string ObjName
    {
        get => transform.name;
        set => transform.name = value;
    }

    void Update()
    {
        AssignOwnerObj();
        OnPlayerLost();
    }

    #region AssignOwnerObj
    void AssignOwnerObj()
    {
        if (_OwnerInfo.OwenrUserId != null && _OwnerInfo.OwnerObj == null)
        {
            Player LocalPlayer = Array.Find(PhotonNetwork.PlayerList, _LocalPlayer => _LocalPlayer == PhotonNetwork.CurrentRoom.GetPlayer(_OwnerInfo.OwnerActorNumber));

            if (LocalPlayer != null && LocalPlayer.TagObject != null)
            {
                _OwnerInfo.OwnerObj = LocalPlayer.TagObject as GameObject;
            }
        }
    }
    #endregion

    #region GameObjectActivity
    public void VoteFXActivity(bool isActive, bool isClicked)
    {
        if (_GameObjects.VoteFX.activeInHierarchy != isActive) _GameObjects.VoteFX.SetActive(isActive);
        if ( isClicked) _GameObjects.VoteFxExplosion.SetActive(true);
    }
    #endregion

    #region GameobjectActivityForAllRoleButtons
    public void VoteFXActivityForAllRoleButton(bool isActive)
    {
        foreach (var roleButtonController in FindObjectsOfType<RoleButtonController>())
        {
            if (roleButtonController._GameObjects.VoteFX.activeInHierarchy != isActive) roleButtonController._GameObjects.VoteFX.SetActive(isActive);
        }
    }
    #endregion

    #region VotedNameIconActivity
    public void VotedNameIconActivity(bool isActive, string votedName)
    {
        _UI.VoteName = votedName;

        if (isActive == true && !String.IsNullOrEmpty(votedName) && _GameObjects.votedNameIconObj.activeInHierarchy != true)
        {
            _GameObjects.votedNameIconObj.SetActive(true);
            FindObjectOfType<UISoundsInGame>().PlaySoundFX(1);
        }
        if (isActive == false)
        {
            _GameObjects.votedNameIconObj.SetActive(false);
        }                 
    }
    #endregion

    #region OnPlayerLost
    void OnPlayerLost()
    {
        if(!_GameInfo.IsPlayerAlive && _GameObjects.DiedIcon.activeInHierarchy == false)
        {
            _UI.VisibleToEveryoneImage = _UI.RoleImage;
            _GameObjects.DiedIcon.SetActive(true);
            _GameObjects.DiedIcon2.SetActive(true);
            _GameObjects.DiedGoreExplosion.SetActive(true);
        }
    }
    #endregion
}
