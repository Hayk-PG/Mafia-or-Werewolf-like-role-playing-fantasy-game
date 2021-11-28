using System;
using System.Collections;

public class MedicNightAction : DayVoteLogic
{
    public override void Vote(SinglePlayRoleButton owner, Action<SinglePlayRoleButton> Vote, Action<bool> RandomVote, Action<SinglePlayRoleButton> SheriffVote)
    {
        StartCoroutine(VoteCoroutine(owner, delegate 
        {
            SinglePlayRoleButton playerWhomWeWantToHeal = null;

            if(IncludedPlayers.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, IncludedPlayers.Count);
                playerWhomWeWantToHeal = IncludedPlayers[index];
            }

            if(playerWhomWeWantToHeal != null)
            {
                Vote?.Invoke(playerWhomWeWantToHeal);
                SheriffVote?.Invoke(playerWhomWeWantToHeal);
            }
            else
            {
                RandomVote?.Invoke(false);
            }
        }));
    }

    protected override IEnumerator VoteCoroutine(SinglePlayRoleButton owner, Action Vote)
    {
        ResetLists(); yield return null;

        InspectInActivePlayers(owner);

        Vote?.Invoke();
    }

    void InspectInActivePlayers(SinglePlayRoleButton owner)
    {
        LoopActivePlayers(
            ActivePlayer => 
            {
                LoopDayVoteData(
                    DayVoteData => 
                    {
                        if(DayVoteData.Key == ActivePlayer && IsOtherPlayer(DayVoteData.Key, owner))
                        {
                            LoopDayVoteDataValue(DayVoteData, 
                                DayVoteDataValue => 
                                {
                                    if(DayVoteDataValue.Value != owner)
                                    {
                                        IncludePlayers(ActivePlayer);
                                    }
                                    else
                                    {
                                        RemoveFromList(ActivePlayer, IncludedPlayers);
                                    }

                                    LoopLostPlayers(
                                        LostPlayer => 
                                        {
                                            if(DayVoteDataValue.Value == LostPlayer && LostPlayer.RoleName == RoleNames.Infected)
                                            {
                                                IncludePlayers(ActivePlayer);
                                            }
                                        });
                                });
                        }
                    });
            });
    }
}
