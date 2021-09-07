using Photon.Pun;
using System.Collections.Generic;

public class GameManagerObserver : MonoBehaviourPun, IPunObservable
{
    GameManagerTimer _GameManagerTimer;
    GameStartAnnouncement _GameStartAnnouncement;
    GameManagerStartTheGame _GameManagerStartTheGame;
    GameManagerSetPlayersRoles _GameManagerSetPlayersRoles;
    GameManagerPlayerVotesController _GameManagerPlayerVotesController;
    TeamsController _TeamsController;


    void Start()
    {
        _GameManagerTimer = GetComponent<GameManagerTimer>();
        _GameStartAnnouncement = GetComponent<GameStartAnnouncement>();
        _GameManagerStartTheGame = GetComponent<GameManagerStartTheGame>();
        _GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _GameManagerPlayerVotesController = GetComponent<GameManagerPlayerVotesController>();
        _TeamsController = GetComponent<TeamsController>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (info.Sender.IsMasterClient)
            {
                #region GameStartAnnouncement
                stream.SendNext(_GameStartAnnouncement._Timer.Seconds);
                stream.SendNext(_GameStartAnnouncement._Timer.GameStartAnnouncementText);
                stream.SendNext(_GameStartAnnouncement._Timer.IsTimeToStartTheGame);
                stream.SendNext(_GameStartAnnouncement._Timer.IsMinRequiredCountReached);
                #endregion

                #region GameManagerTimer
                stream.SendNext(_GameManagerTimer._Timer.Seconds);
                stream.SendNext(_GameManagerTimer._Timer.TimerText);
                stream.SendNext(_GameManagerTimer._Timer.NightsCount);
                stream.SendNext(_GameManagerTimer._Timer.NightTime);
                stream.SendNext(_GameManagerTimer._Timer.DaysCount);
                stream.SendNext(_GameManagerTimer._Timer.DayTime);
                stream.SendNext(_GameManagerTimer._Timer.HasGameStartVFXInstantiated);
                stream.SendNext(_GameManagerTimer._LostPlayer.HasLostPlayerSet);
                stream.SendNext(_GameManagerTimer._LostPlayer.LostPlayers);
                stream.SendNext(_GameManagerTimer._Teams.IsTeamsCountSet);
                stream.SendNext(_GameManagerTimer._Teams.IsTeamsCountUpdated);
                #endregion

                #region GameManagerStartTheGame
                stream.SendNext(_GameManagerStartTheGame._GameStart.GameStarted);
                #endregion

                #region GameManagerSetPlayersRoles
                stream.SendNext(_GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet);
                #endregion

                #region GameManagerPlayerVotesController
                stream.SendNext(_GameManagerPlayerVotesController._Votes.PlayersVotesAgainst);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.PlayerVoteCondition);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.HealedPlayers);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.DiscoverTheRole);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.InfectedVotesAgainst);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.SoldierVoteAgainst);
                stream.SendNext(_GameManagerPlayerVotesController._Votes.LizardVoteAgainst);
                #endregion

                #region TeamsController
                stream.SendNext(_TeamsController._TeamsCount.Team);
                stream.SendNext(_TeamsController._TeamsCount.FirstTeamCount);
                stream.SendNext(_TeamsController._TeamsCount.SecondTeamCount);
                #endregion
            }
        }
        else
        {
            #region GameStartAnnouncement
            _GameStartAnnouncement._Timer.Seconds = (int)stream.ReceiveNext();
            _GameStartAnnouncement._Timer.GameStartAnnouncementText = (string)stream.ReceiveNext();
            _GameStartAnnouncement._Timer.IsTimeToStartTheGame = (bool)stream.ReceiveNext();
            _GameStartAnnouncement._Timer.IsMinRequiredCountReached = (bool)stream.ReceiveNext();
            #endregion

            #region GameManagerTimer
            _GameManagerTimer._Timer.Seconds = (int)stream.ReceiveNext();
            _GameManagerTimer._Timer.TimerText = (string)stream.ReceiveNext();
            _GameManagerTimer._Timer.NightsCount = (int)stream.ReceiveNext();
            _GameManagerTimer._Timer.NightTime = (bool)stream.ReceiveNext();
            _GameManagerTimer._Timer.DaysCount = (int)stream.ReceiveNext();
            _GameManagerTimer._Timer.DayTime = (bool)stream.ReceiveNext();
            _GameManagerTimer._Timer.HasGameStartVFXInstantiated = (bool)stream.ReceiveNext();
            _GameManagerTimer._LostPlayer.HasLostPlayerSet = (bool)stream.ReceiveNext();
            _GameManagerTimer._LostPlayer.LostPlayers = (Dictionary<int, bool>)stream.ReceiveNext();
            _GameManagerTimer._Teams.IsTeamsCountSet = (bool)stream.ReceiveNext();
            _GameManagerTimer._Teams.IsTeamsCountUpdated = (bool)stream.ReceiveNext();
            #endregion

            #region GameManagerStartTheGame
            _GameManagerStartTheGame._GameStart.GameStarted = (bool)stream.ReceiveNext();
            #endregion

            #region GameManagerSetPlayersRoles
            _GameManagerSetPlayersRoles._Condition.HasPlayersRolesBeenSet = (bool)stream.ReceiveNext();
            #endregion

            #region GameManagerPlayerVotesController
            _GameManagerPlayerVotesController._Votes.PlayersVotesAgainst = (Dictionary<int, int>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.PlayerVoteCondition = (Dictionary<int, bool[]>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.AgainstWhomPlayerVoted = (Dictionary<int, string>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.HealedPlayers = (Dictionary<int, bool>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.DiscoverTheRole = (Dictionary<int, bool>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.InfectedVotesAgainst = (Dictionary<int, int>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.SoldierVoteAgainst = (Dictionary<int, int>)stream.ReceiveNext();
            _GameManagerPlayerVotesController._Votes.LizardVoteAgainst = (Dictionary<int, bool>)stream.ReceiveNext();
            #endregion

            #region TeamsController
            _TeamsController._TeamsCount.Team = (Dictionary<int, string>)stream.ReceiveNext();
            _TeamsController._TeamsCount.FirstTeamCount = (int)stream.ReceiveNext();
            _TeamsController._TeamsCount.SecondTeamCount = (int)stream.ReceiveNext();
            #endregion
        }
    }
}
