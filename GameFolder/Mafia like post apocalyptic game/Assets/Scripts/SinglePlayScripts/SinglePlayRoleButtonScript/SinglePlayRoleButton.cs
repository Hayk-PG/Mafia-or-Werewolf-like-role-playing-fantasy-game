using System;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayRoleButton : MonoBehaviour
{
    [Serializable] public class Info
    {
        [SerializeField] internal Text nameText;
        [SerializeField] internal Text votesCountText;
        [SerializeField] internal Image roleImage;
        [SerializeField] internal Sprite actualRoleSprite;
        [SerializeField] internal string roleName;
        [SerializeField] internal bool isPlayer;
        [SerializeField] internal bool isAlive;
    }
    [Serializable] public class GameObjects
    {
        [SerializeField] GameObject voteVFX;
        [SerializeField] GameObject selected;

        public GameObject VoteVFX
        {
            get => voteVFX;
        }
        public GameObject Selected
        {
            get => selected;
        }
    }

    public Info _Info;
    public GameObjects _GameObjects;

    public string Name
    {
        get => _Info.nameText.text;
        set => _Info.nameText.text = value;
    }
    public string RoleName
    {
        get => _Info.roleName;
        set => _Info.roleName = value;
    }
    public string VotesCount
    {
        get => _Info.votesCountText.text;
        set => _Info.votesCountText.text = value;
    }
    public Sprite RoleSprite
    {
        get => _Info.actualRoleSprite;
        set => _Info.actualRoleSprite = value;
    }
    public Sprite RoleImage
    {
        get => _Info.roleImage.sprite;
        set => _Info.roleImage.sprite = value;
    }
    public bool IsPlayer
    {
        get => _Info.isPlayer;
        set => _Info.isPlayer = value;
    }
    public bool IsAlive
    {
        get => _Info.isAlive;
        set => _Info.isAlive = value;
    }

    public void VotesGameObjectActivity(bool isActive)
    {
        if(_Info.votesCountText.gameObject.activeInHierarchy != isActive) _Info.votesCountText.gameObject.SetActive(isActive);
    }
}
