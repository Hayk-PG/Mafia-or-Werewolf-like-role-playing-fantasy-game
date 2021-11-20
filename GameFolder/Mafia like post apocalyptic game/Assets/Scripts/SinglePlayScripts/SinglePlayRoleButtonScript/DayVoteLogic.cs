using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayVoteLogic : VotingBaseScript
{    

    public override void Vote(SinglePlayRoleButton owner, Action<SinglePlayRoleButton> Vote, Action<bool> RandomVote, Action<SinglePlayRoleButton> SheriffVote)
    {       
        StartCoroutine(VoteCoroutine(owner, delegate 
        {
            SinglePlayRoleButton suspect = null;

            if (MainSuspects.Count < 1 && PrioritizedPlayers.Count < 1 && IncludedPlayers.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, IncludedPlayers.Count);
                suspect = IncludedPlayers[index];
            }
            if (MainSuspects.Count < 1 && PrioritizedPlayers.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, PrioritizedPlayers.Count);
                suspect = PrioritizedPlayers[index];
            }
            if (MainSuspects.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, MainSuspects.Count);
                suspect = MainSuspects[index];
            }

            print(MainSuspects.Count + "/" + PrioritizedPlayers.Count + "/" + IncludedPlayers.Count + "/" + suspect);

            if(suspect != null)
            {
                Vote?.Invoke(suspect);
                SheriffVote?.Invoke(suspect);
            }
            else
            {
                RandomVote?.Invoke(false);
            }
        }));
    }

    IEnumerator VoteCoroutine(SinglePlayRoleButton owner, Action Vote)
    {
        IncludedPlayers = new List<SinglePlayRoleButton>(); yield return null;
        PrioritizedPlayers = new List<SinglePlayRoleButton>(); yield return null;
        MainSuspects = new List<SinglePlayRoleButton>(); yield return null;

        LoopLostPlayers(LostPlayer => 
        {
            if (!SinglePlayGlobalConditions.IsAiInfected(LostPlayer))
            {
                InspectOthersInLostPlayersList(owner, LostPlayer);
            }
            else
            {
                InspectInfectedsInLostPlayersList(LostPlayer);              
            }
        }); yield return null;

        LoopActivePlayers(VotedAgainstUsPlayer => 
        {
            DetectPlayersWhoVotedAgainstUs(owner, VotedAgainstUsPlayer);
        }); yield return null;

        LoopActivePlayers(HighestVoteReceivedPlayer => 
        {
            DetermineHighestVotesReceivedPlayers(owner, HighestVoteReceivedPlayer);
        }); yield return null;

        Vote?.Invoke();
    }

    void InspectInfectedsInLostPlayersList(SinglePlayRoleButton LostPlayer)
    {
        LoopDayVoteData(DayVoteData =>
        {
            LoopDayVoteDataValue(DayVoteData,
                Value =>
                {
                    if (Value.Value == LostPlayer)
                    {
                        RemoveFromList(DayVoteData.Key, IncludedPlayers);
                    }
                });

            if (DayVoteData.Key == LostPlayer)
            {
                LoopDayVoteDataValue(DayVoteData, Value =>
                {
                    RemoveFromList(Value.Value, IncludedPlayers);
                });
            }
        });
    }

    void InspectOthersInLostPlayersList(SinglePlayRoleButton owner, SinglePlayRoleButton LostPlayer)
    {
        LoopDayVoteData(DayVoteData =>
        {
            LoopDayVoteDataValue(DayVoteData,
                Value =>
                {
                    if (Value.Value == LostPlayer)
                    {
                        if (Value.Key == PreviousDaysCount)
                        {
                            if (IsOtherPlayer(DayVoteData.Key, owner) && IsPlayerALive(DayVoteData.Key)) AddPrioritizedPlayers(DayVoteData.Key);
                        }
                        else if (Value.Key == DaysCount)
                        {
                            if (IsOtherPlayer(DayVoteData.Key, owner) && IsPlayerALive(DayVoteData.Key)) IncludePlayers(DayVoteData.Key);
                        }
                    }
                });

            if (DayVoteData.Key == LostPlayer)
            {
                LoopDayVoteDataValue(DayVoteData, 
                    Value => 
                    {
                        if (IsOtherPlayer(Value.Value, owner) && IsPlayerALive(Value.Value)) IncludePlayers(Value.Value);
                    });
            }
        });
    }

    void DetectPlayersWhoVotedAgainstUs(SinglePlayRoleButton owner, SinglePlayRoleButton VotedAgainstUsPlayer)
    {
        LoopDayVoteData(DayVoteData =>
        {
            if (DayVoteData.Key == VotedAgainstUsPlayer)
            {
                LoopDayVoteDataValue(DayVoteData, Value =>
                {
                    if (Value.Value == owner)
                    {
                        if (IsPlayerALive(DayVoteData.Key)) IncludePlayers(DayVoteData.Key);
                    }
                });
            }
        });
    }

    void DetermineHighestVotesReceivedPlayers(SinglePlayRoleButton owner, SinglePlayRoleButton HighestVoteReceivedPlayer)
    {
        if (HighestVoteReceivedPlayer.VotesCount >= 3)
        {
            LoopDayVoteData(DayVoteData =>
            {
                LoopDayVoteDataValue(DayVoteData, Value =>
                {
                    if (Value.Value == HighestVoteReceivedPlayer)
                    {
                        if (IsOtherPlayer(DayVoteData.Key, owner))
                        {                            
                            IncludePlayers(DayVoteData.Key);

                            if (PrioritizedPlayers.Contains(DayVoteData.Key))
                            {
                                AddMainSuspectPlayers(DayVoteData.Key);
                            }
                        }
                    }
                });
            });
        }
    }
}
