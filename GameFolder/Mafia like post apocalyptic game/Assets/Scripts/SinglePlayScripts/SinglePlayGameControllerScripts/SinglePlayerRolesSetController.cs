using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerRolesSetController : MonoBehaviour
{
    internal class RolesInfo
    {
        internal SinglePlayGameController.RolesClass _RolesClass { get; set; }
        internal SinglePlayGameController.RolesImagesClass _RolesImagesClass { get; set; }
        internal int Index { get; set; }
        internal int RandomRange { get; set; }
        internal string RandomName { get; set; }
        internal Sprite RoleSprite { get; set; }

        internal RolesInfo()
        {

        }

        internal RolesInfo(SinglePlayGameController.RolesClass _RolesClass, SinglePlayGameController.RolesImagesClass _RolesImagesClass, int Index, int RandomRange, string RandomName, Sprite RoleSprite)
        {
            this._RolesClass = _RolesClass;
            this._RolesImagesClass = _RolesImagesClass;
            this.Index = Index;
            this.RandomRange = RandomRange;
            this.RandomName = RandomName;
            this.RoleSprite = RoleSprite;
        }
    }

    SinglePlayGameController _SinglePlayGameController { get; set; }


    void Awake()
    {
        _SinglePlayGameController = GetComponent<SinglePlayGameController>();
    }

    internal void HideAllRoleButtons(RolesInfo rolesInfo)
    {
        foreach (var roleButton in rolesInfo._RolesClass.RoleButtons)
        {
            roleButton.gameObject.SetActive(false);
        }
    }

    internal void GettingRandomNumber(out List<int> random)
    {
        random = new List<int>();

        for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
        {
            random.Add(i);
        }
    }

    internal void RemoveRandoms(List<int> random, RolesInfo rolesInfo)
    {
        random.Remove(rolesInfo.RandomRange);
    }

    internal void SettingRandomRange(RolesInfo rolesInfo, List<int> random, out int randomRange)
    {
        randomRange = _SinglePlayGameController.test && rolesInfo.Index == 0 ? 11 : random[Random.Range(0, random.Count)];
    }

    internal void SettingRandomNames(SinglePlayGameController.RolesClass _RolesClass, out string randomName)
    {
        randomName = _RolesClass.MalePlayersNames[Random.Range(0, _RolesClass.MalePlayersNames.Count)];
    }

    internal void SettingRoles(RolesInfo rolesInfo)
    {
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].gameObject.SetActive(true);
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].Name = rolesInfo.Index == 0 ? "You" : rolesInfo.RandomName;
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].gameObject.SetActive(true);
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleName = rolesInfo._RolesClass.PlayersRolesNames[rolesInfo.RandomRange];
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleImage = rolesInfo.Index == 0 ? rolesInfo.RoleSprite : rolesInfo._RolesImagesClass.DefaultSprite[0];
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleSprite = rolesInfo.RoleSprite;
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].IsPlayer = rolesInfo.Index == 0 ? true : false;
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].IsAlive = true;
    }

    internal void IfPlayerIsInfected(RolesInfo rolesInfo)
    {
        if (rolesInfo._RolesClass.RoleButtons[0].IsPlayer && rolesInfo._RolesClass.RoleButtons[0].RoleName == RoleNames.Infected)
        {
            if (rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleName == RoleNames.Infected)
            {
                rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleImage = rolesInfo.RoleSprite;
            }
        }
    }

    internal void DefineRoleNames(RolesInfo rolesInfo)
    {
        rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleNameText = rolesInfo.Index == 0 ? 
            rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleName : rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleName == RoleNames.Infected &&
                rolesInfo._RolesClass.RoleButtons[0].RoleName == RoleNames.Infected ? 
                rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleNameText = rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleName :
                rolesInfo._RolesClass.RoleButtons[rolesInfo.Index].RoleNameText = "Unknown";
    }

    internal void RemoveMaleNames(RolesInfo rolesInfo)
    {
        rolesInfo._RolesClass.MalePlayersNames.Remove(rolesInfo.RandomName);
    }
}
