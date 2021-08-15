using Photon.Pun;

public class GetTeamsUI : MonoBehaviourPun,IUpdateTeamsCount
{
    void OnEnable()
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerBaseConditions.MyComponents.PlayerEvents.OnSetTeamsUI += PlayerEvents_OnSetTeamsUI;
        }
    }
  
    void OnDisable()
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerBaseConditions.MyComponents.PlayerEvents.OnSetTeamsUI -= PlayerEvents_OnSetTeamsUI;
        }
    }

    void PlayerEvents_OnSetTeamsUI()
    {
        int humansCount = 0;
        int infectedsCount = 0;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if(PlayerBaseConditions._PlayerTagObject(player.ActorNumber).GetComponent<ISetPlayerRoleProps>().RoleName != RoleNames.Infected && PlayerBaseConditions._PlayerTagObject(player.ActorNumber).GetComponent<ISetPlayerRoleProps>().RoleName != RoleNames.Lizard)
            {
                humansCount++;
            }
            else
            {
                infectedsCount++;
            }
        }

        PlayerBaseConditions._MyGameControllerComponents.GameUITexts.HumansCount = humansCount;
        PlayerBaseConditions._MyGameControllerComponents.GameUITexts.InfectedsCount = infectedsCount;
    }

    /// <summary>
    /// If true, reduce infecteds count,else reduce humans count
    /// </summary>
    /// <param name="obj"></param>
    public void UpdateTeamsCount(bool isInfectedsTeam)
    {
        if (isInfectedsTeam)
        {
            PlayerBaseConditions._MyGameControllerComponents.GameUITexts.Anims[1].SetTrigger("play");
            PlayerBaseConditions._MyGameControllerComponents.GameUITexts.InfectedsCount--;
        }
        else
        {
            PlayerBaseConditions._MyGameControllerComponents.GameUITexts.Anims[0].SetTrigger("play");
            PlayerBaseConditions._MyGameControllerComponents.GameUITexts.HumansCount--;
        }
    }




}
