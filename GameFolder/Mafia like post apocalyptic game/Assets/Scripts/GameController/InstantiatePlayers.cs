using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InstantiatePlayers : MonoBehaviourPun
{  
    [Header("PREFAB NAME")]
    [SerializeField] string playerPrefabsName;

    [Header("AVATAR BUTTONS CONTROLLER")]
    [SerializeField] AvatarButtonController[] avatarButtonController;

    #region AVATAR SPRITES
    [Header("ROLES IMAGES")]
    [SerializeField] List<Sprite> doctorSprite;
    [SerializeField] List<Sprite> sheriffSprite;
    [SerializeField] List<Sprite> soldiersSprite;
    [SerializeField] List<Sprite> citizensSprite;
    [SerializeField] List<Sprite> lizardsSprites;
    [SerializeField] List<Sprite> infectedsSprites;
    [SerializeField] List<Sprite> baseAvatars;

    /// <summary>
    /// Female doctor
    /// </summary>
    public List<Sprite> DoctorAvatar => doctorSprite;

    /// <summary>
    /// Male sheriff
    /// </summary>
    public List<Sprite> SheriffAvatar => sheriffSprite;

    /// <summary>
    /// 0:Male soldier 1:Male soldier 2:Female soldier
    /// </summary>
    public List<Sprite> SoldiersAvatar => soldiersSprite;

    /// <summary>
    /// 0: Female citizen 1: Female citizen 2:Male citizen
    /// </summary>
    public List<Sprite> CitizenAvatar => citizensSprite;

    /// <summary>
    /// 0:Female lizard 1:Female lizard
    /// </summary>
    public List<Sprite> LizardAvatar => lizardsSprites;

    /// <summary>
    /// 0:Male infected 1:Male infected 2:Female infected
    /// </summary>
    public List<Sprite> InfectedAvatar => infectedsSprites;

    /// <summary>
    /// 0:Male 1:Female
    /// </summary>
    public List<Sprite> BaseAvatar => baseAvatars;
    #endregion

    public AvatarButtonController[] AvatarButtonController => avatarButtonController;

    [Header("ROLE NAMES")]
    public List<string> PlayersRolesNames = new List<string>(20)
    {
        RoleNames.Citizen, RoleNames.Citizen, RoleNames.Infected, RoleNames.Medic, RoleNames.Infected, RoleNames.Soldier, RoleNames.Infected, RoleNames.Citizen,
        RoleNames.Lizard, RoleNames.Sheriff, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected,
        RoleNames.Citizen, RoleNames.Infected, RoleNames.Soldier, RoleNames.Lizard
    };

    public int PlayersCount
    {
        get
        {
            return PhotonNetwork.PlayerList.Length;
        }
    }




    void Start()
    {
        PlayerInstantiation();
    }
   
    void PlayerInstantiation()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefabsName, Vector3.zero, Quaternion.identity);
    }

    #region Roles reference
    public void SetPlayerRoleAvatar(string roleName, SetPlayerInfo playerInfo, int avatarButtonIndex, int GenderRandomNumber, bool isForInfectedMembers)
    {
        switch (roleName)
        {
            case RoleNames.Citizen: ForCitizenRole(playerInfo, avatarButtonIndex, GenderRandomNumber); break;
            case RoleNames.Medic: ForMedicRole(PlayersCount, playerInfo, avatarButtonIndex); break;
            case RoleNames.Sheriff: FoSheriffRole(PlayersCount, playerInfo, avatarButtonIndex); break;
            case RoleNames.Soldier: ForSoldierRole(playerInfo, avatarButtonIndex, GenderRandomNumber); break;
            case RoleNames.Infected: ForInfectedRole(playerInfo, avatarButtonIndex, GenderRandomNumber, isForInfectedMembers); break;
            case RoleNames.Lizard: ForLizardRole(playerInfo, avatarButtonIndex, GenderRandomNumber); break;
        }
    }

    void ForCitizenRole(SetPlayerInfo playerInfo, int index, int GenderRandomNumber)
    {
        if (playerInfo.playerGender == SetPlayerInfo.PlayerGender.Male)
        {
            AvatarButtonController[index].HiddenAvatarSprite = CitizenAvatar[2];
        }
        else
        {
            GenderRandomNumber = Random.Range(0, 1);
            AvatarButtonController[index].HiddenAvatarSprite = CitizenAvatar[GenderRandomNumber];
        }

        BaseAvatars(index, playerInfo.playerGender);
    }

    void ForMedicRole(int playersCount, SetPlayerInfo playerInfo, int index)
    {
        AvatarButtonController[index].HiddenAvatarSprite = DoctorAvatar[0];

        BaseAvatars(index, playerInfo.playerGender);
    }

    void FoSheriffRole(int playersCount, SetPlayerInfo playerInfo, int index)
    {
        AvatarButtonController[index].HiddenAvatarSprite = SheriffAvatar[0];

        BaseAvatars(index, playerInfo.playerGender);
    }

    void ForSoldierRole(SetPlayerInfo playerInfo, int index, int GenderRandomNumber)
    {
        if (playerInfo.playerGender == SetPlayerInfo.PlayerGender.Male)
        {
            AvatarButtonController[index].HiddenAvatarSprite = SoldiersAvatar[GenderRandomNumber];
        }
        else
        {
            AvatarButtonController[index].HiddenAvatarSprite = SoldiersAvatar[2];
        }

        BaseAvatars(index, playerInfo.playerGender);
    }

    void ForInfectedRole(SetPlayerInfo playerInfo, int index, int GenderRandomNumber,bool isForTeamMembers)
    {
        if (playerInfo.playerGender == SetPlayerInfo.PlayerGender.Male)
        {           
            if (isForTeamMembers)
            {
                AvatarButtonController[index].AvatarSprite = InfectedAvatar[GenderRandomNumber];                
            }
            else
            {
                BaseAvatars(index, playerInfo.playerGender);
            }

            AvatarButtonController[index].HiddenAvatarSprite = InfectedAvatar[GenderRandomNumber];
        }
        else
        {            
            if (isForTeamMembers)
            {
                AvatarButtonController[index].AvatarSprite = InfectedAvatar[2];               
            }
            else
            {
                BaseAvatars(index, playerInfo.playerGender);
            }

            AvatarButtonController[index].HiddenAvatarSprite = InfectedAvatar[2];
        }
    }

    void ForLizardRole(SetPlayerInfo playerInfo, int index, int GenderRandomNumber)
    {
        AvatarButtonController[index].HiddenAvatarSprite = LizardAvatar[GenderRandomNumber];

        BaseAvatars(index, playerInfo.playerGender);
    }

    void BaseAvatars(int index, SetPlayerInfo.PlayerGender Gender)
    {
        AvatarButtonController[index].AvatarSprite = Gender == SetPlayerInfo.PlayerGender.Male ? BaseAvatar[0] : BaseAvatar[1];
    }
    #endregion









}
