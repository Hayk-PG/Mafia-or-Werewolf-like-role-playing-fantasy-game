using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayVoteDatas : MonoBehaviour
{
    public class DayVotesInfo
    {
        public string OtherPlayerName { get; set; }
        public int DaysCount { get; set; }

        public DayVotesInfo(string OtherPlayerName, int DaysCount)
        {
            this.OtherPlayerName = OtherPlayerName;
            this.DaysCount = DaysCount;
        }
    }
    public Dictionary<string, DayVotesInfo> InfectedsDayVotesInfo { get; set; }
    public Dictionary<SinglePlayRoleButton, Dictionary<int, SinglePlayRoleButton>> DayVotesData { get; set; }

    SinglePlayGameController _SinglePlayGameController { get; set; }


    void Awake()
    {
        InfectedsDayVotesInfo = new Dictionary<string, DayVotesInfo>();
        DayVotesData = new Dictionary<SinglePlayRoleButton, Dictionary<int, SinglePlayRoleButton>>();

        _SinglePlayGameController = GetComponent<SinglePlayGameController>();
    }

    void OnEnable()
    {
        _SinglePlayGameController.OnPhaseReset += _SinglePlayGameController_OnPhaseReset;
    }   

    void OnDisable()
    {
        _SinglePlayGameController.OnPhaseReset -= _SinglePlayGameController_OnPhaseReset;
    }

    void _SinglePlayGameController_OnPhaseReset(bool obj)
    {
        InfectedsDayVotesInfo = new Dictionary<string, DayVotesInfo>();
    }

    public void AddInfectedsDayVotesInfo(string infectedRole, DayVotesInfo votesInfo)
    {
        if(InfectedsDayVotesInfo.ContainsKey(infectedRole))
        {
            InfectedsDayVotesInfo[infectedRole] = votesInfo;
        }
        else
        {
            InfectedsDayVotesInfo = new Dictionary<string, DayVotesInfo>()
            {
                {infectedRole,  votesInfo}
            };
        }
    }

    public void AddDayVotesData(SinglePlayRoleButton owner, int daysCount, SinglePlayRoleButton other)
    {
        if (DayVotesData.ContainsKey(owner))
        {
            if(DayVotesData[owner] != null)
            {               
                DayVotesData[owner].Add(daysCount, other);
            }
            else
            {
                Dictionary<int, SinglePlayRoleButton> data = new Dictionary<int, SinglePlayRoleButton>() { { daysCount, other } };
                DayVotesData[owner] = data;
            }
        }
        else
        {
            DayVotesData.Add(owner, new Dictionary<int, SinglePlayRoleButton>() { { daysCount, other } });
        }
    }
}
