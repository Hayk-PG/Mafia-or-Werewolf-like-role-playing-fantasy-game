using UnityEngine;

public interface IPlayerGameController
{
    Photon.Pun.PhotonView PhotonView { get; }
    bool CanPlayerBeActiveInNightPhase { get; set; }
    bool CanPlayerBeActiveInDayPhase { get; set; }
    bool IsPlayerAlive { get; set; }
    bool HasVotePhaseResetted { get; set; }
}
