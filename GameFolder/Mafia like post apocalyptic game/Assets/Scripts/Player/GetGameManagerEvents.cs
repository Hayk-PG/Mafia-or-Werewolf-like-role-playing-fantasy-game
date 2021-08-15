using Photon.Pun;
using System;

public class GetGameManagerEvents : MonoBehaviourPun
{
    public event Action<int> OnClickDayVote;
    public event Action<int> OnClickNightVote;
    public event Action<int, string> OnClickPlayerAvatar;
    public event Action OnClickEmptyAvatar;


    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnClickAvatarButtons += GlobalInputs_OnClickAvatarButtons;           
        });
    }
   
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnClickAvatarButtons -= GlobalInputs_OnClickAvatarButtons;           
        }
    }

    void GlobalInputs_OnClickAvatarButtons(int actorNumber)
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            if (PlayerBaseConditions.IsActorNumberNotNull(actorNumber))
            {
                if (!PlayerBaseConditions.IsActorNumberMine(actorNumber) && PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().IsPlayerStillPlaying && PlayerBaseConditions._CanPlayerVoteGlobal)
                {
                    if (PlayerBaseConditions._IsTimeToNightVote)
                    {
                        OnClickNightVote?.Invoke(actorNumber);

                        PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.CanPlayerVote = false;
                    }
                    if (PlayerBaseConditions._IsTimeToDayVote)
                    {
                        OnClickDayVote?.Invoke(actorNumber);

                        PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.CanPlayerVote = false;
                    }
                }
                if (!PlayerBaseConditions._IsTimeToNightVote && !PlayerBaseConditions._IsTimeToDayVote)
                {
                    OnClickPlayerAvatar?.Invoke(actorNumber, PlayerBaseConditions.PlayerPlayfabId(actorNumber));
                }
            }
            else
            {
                OnClickEmptyAvatar?.Invoke();
            }
        }      
    }

    









}
