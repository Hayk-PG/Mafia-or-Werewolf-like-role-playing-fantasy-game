using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TeamsController : MonoBehaviour
{
    [Serializable] public class TeamsCount
    {
        [SerializeField] Text firstTeamCountText;
        [SerializeField] Text secondTeamCountText;

        public int FirstTeamCount
        {
            get => int.Parse(firstTeamCountText.text);
            set => firstTeamCountText.text = value.ToString();
        }
        public int SecondTeamCount
        {
            get => int.Parse(secondTeamCountText.text);
            set => secondTeamCountText.text = value.ToString();
        }

        public Dictionary<int, string> Team = new Dictionary<int, string>();
    }

    public TeamsCount _TeamsCount;

    public void GetTeamsCount(RoleButtonController[] roleButtons)
    {
        foreach (var roleButton in roleButtons)
        {
            if (!String.IsNullOrEmpty(roleButton._GameInfo.RoleName))
            {
                _TeamsCount.Team.Add(roleButton._OwnerInfo.OwnerActorNumber, roleButton._GameInfo.RoleName);
            }        
        }

        foreach (var teams in _TeamsCount.Team)
        {
            if (teams.Value == RoleNames.Infected || teams.Value == RoleNames.Lizard)
                _TeamsCount.SecondTeamCount++;
            else _TeamsCount.FirstTeamCount++;
        }
    }

    public void UpdateTeamsCount()
    {
        _TeamsCount.SecondTeamCount = 0;
        _TeamsCount.FirstTeamCount = 0;

        foreach (var teams in _TeamsCount.Team)
        {
            RoleButtonController roleButton = Array.Find(FindObjectsOfType<RoleButtonController>(), _roleButton => _roleButton._OwnerInfo.OwnerActorNumber == teams.Key);

            if (roleButton._GameInfo.IsPlayerAlive)
            {
                if (roleButton._GameInfo.RoleName == RoleNames.Infected || roleButton._GameInfo.RoleName == RoleNames.Lizard) _TeamsCount.SecondTeamCount++;
                else _TeamsCount.FirstTeamCount++;
            }
        }
    }
}
