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
    }
    [Serializable] public struct GameInfo
    {
        [SerializeField] string roleName;
        [SerializeField] int roleIndex;
        [SerializeField] bool isPlayerAlive;

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
    }
    [Serializable] public struct GameObjects
    {
        [SerializeField] GameObject[] iconObjs;

        public GameObject[] IconObjs
        {
            get => iconObjs;
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
    public void GameObjectActivity(int index, bool isActive)
    {
        if(index >= 0) _GameObjects.IconObjs[index].SetActive(isActive);
    }
    #endregion

    #region GameobjectActivityForAllRoleButtons
    public void GameobjectActivityForAllRoleButtons(int index, bool isActive)
    {
        foreach (var roleButtonController in FindObjectsOfType<RoleButtonController>())
        {
            if (index >= 0) roleButtonController._GameObjects.IconObjs[index].SetActive(isActive);
        }
    }
    #endregion
}
