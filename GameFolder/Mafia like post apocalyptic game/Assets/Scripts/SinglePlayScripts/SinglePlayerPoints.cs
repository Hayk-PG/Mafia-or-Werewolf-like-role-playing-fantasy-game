using UnityEngine;

public class SinglePlayerPoints : MonoBehaviour
{
    [SerializeField] int points;
    public int Points
    {
        get => points;
        set => points = value;
    }

    public class Data
    {
        public SinglePlayRoleButton Player { get; set; }
        public SinglePlayRoleButton AI { get; set; }
        public int Points { get; set; }
        public int PunishmentPoints { get; set; }

        public Data(SinglePlayRoleButton Player, SinglePlayRoleButton AI, int Points, int PunishmentPoints)
        {
            this.Player = Player;
            this.AI = AI;
            this.Points = Points;
            this.PunishmentPoints = PunishmentPoints;
        }
    }

    public void PointsForDayVote(Data data)
    {
        if (SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.Player) && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.Player) && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
        if (!SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.Player) && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (!SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.Player) && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForMedic(Data data)
    {
        if (SinglePlayGlobalConditions.AmIMedic() && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.AmIMedic() && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForSheriff(Data data)
    {
        if (SinglePlayGlobalConditions.AmISheriff() && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.AmISheriff() && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForSoldier(Data data)
    {
        if (SinglePlayGlobalConditions.AmISoldier() && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.AmISoldier() && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForInfected(Data data)
    {
        if (SinglePlayGlobalConditions.AmIInfected() && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.AmIInfected() && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForLizard(Data data)
    {
        if (SinglePlayGlobalConditions.AmILizard() && SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.Points;
        if (SinglePlayGlobalConditions.AmILizard() && !SinglePlayGlobalConditions.IsPlayerInHumansTeam(data.AI))
            Points += data.PunishmentPoints;
    }

    public void PointsForStayingAlive(Data data)
    {
        if(data.Player.IsAlive)
            Points += data.Points;
    }
}
