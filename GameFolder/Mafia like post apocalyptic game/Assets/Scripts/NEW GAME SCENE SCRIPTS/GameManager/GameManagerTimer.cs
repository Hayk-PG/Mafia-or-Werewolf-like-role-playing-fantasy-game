using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerTimer : MonoBehaviourPun
{
    [Serializable] public class Timer
    {
        [SerializeField] int seconds;
        [SerializeField] int nightsCount;
        [SerializeField] int daysCount;
        [SerializeField] bool nightTime;
        [SerializeField] bool dayTime;
        [SerializeField] bool hasGameStartVFXInstantiated;
        [SerializeField] Text timerText;
        [SerializeField] GameObject sun;
        [SerializeField] GameObject moon;
       
        public int Seconds
        {
            get => seconds;
            set => seconds = value;
        }
        public int NightsCount
        {
            get => nightsCount;
            set => nightsCount = value;
        }
        public int DaysCount
        {
            get => daysCount;
            set => daysCount = value;
        }
        public bool NightTime
        {
            get => nightTime;
            set => nightTime = value;
        }
        public bool DayTime
        {
            get => dayTime;
            set => dayTime = value;
        }
        public bool HasGameStartVFXInstantiated
        {
            get => hasGameStartVFXInstantiated;
            set => hasGameStartVFXInstantiated = value;
        }
        public string TimerText
        {
            get => timerText.text;
            set => timerText.text = value;
        }
        public GameObject Sun
        {
            get => sun;
        }
        public GameObject Moon
        {
            get => moon;
        }

        internal GameManagerVFXHolder VfxHolder { get; set; }
        internal GameManagerSetPlayersRoles GameManagerSetPlayersRoles { get; set; }
    }

    public Timer _Timer;
    UISoundsInGame _UISoundsInGame;

    const byte CreateVFX = 0;
    const byte EverySecond = 1;

    void Awake()
    {
        _Timer.VfxHolder = GetComponent<GameManagerVFXHolder>();
        _Timer.GameManagerSetPlayersRoles = GetComponent<GameManagerSetPlayersRoles>();
        _UISoundsInGame = FindObjectOfType<UISoundsInGame>();
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    void Start()
    {
        _Timer.NightTime = true;
        _Timer.Moon.SetActive(true);
        _Timer.Sun.SetActive(false);
    }

    void Update()
    {
        if (_Timer.Moon.activeInHierarchy != _Timer.NightTime) _Timer.Moon.SetActive(_Timer.NightTime);
        if (_Timer.Sun.activeInHierarchy != _Timer.DayTime) _Timer.Sun.SetActive(_Timer.DayTime);
    }

    void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (obj.Code == CreateVFX)
        {
            OnCreateVFX(obj);
        }
        if (obj.Code == EverySecond)
        {
            OnPlaySoundFX(obj);

            OnPlayerActivity(obj);
        }
    }
   
    internal void RunTimer()
    {
        StartCoroutine(TimerCoroutine(_Timer.Seconds));
    }

    IEnumerator TimerCoroutine(int currentSeconds)
    {
        while (photonView.IsMine)
        {
            _Timer.Seconds = currentSeconds;

            object[] datas = new object[] 
            {
                "CreateVFX",
                "PlaySoundFX",
                "NightVote" ,
                "DayVote" ,
                "ResetVotesConditions",
                "PlayerActivity"
            };

            PhotonNetwork.RaiseEvent(CreateVFX, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendReliable);

            while (true && photonView.IsMine)
            {
                if (_Timer.NightTime && _Timer.Seconds <= 0)
                {
                    _Timer.Seconds = 90;
                    _Timer.NightsCount++;
                    _Timer.NightTime = false;
                    _Timer.DayTime = true;
                }
                if (_Timer.DayTime && _Timer.Seconds <= 0)
                {
                    _Timer.Seconds = 60;
                    _Timer.DaysCount++;
                    _Timer.DayTime = false;
                    _Timer.NightTime = true;
                }

                _Timer.Seconds--;
                _Timer.TimerText = _Timer.Seconds.ToString("D2");

                PhotonNetwork.RaiseEvent(EverySecond, datas, new RaiseEventOptions { Receivers = ReceiverGroup.All }, ExitGames.Client.Photon.SendOptions.SendUnreliable);

                yield return new WaitForSeconds(1);
            }
        }
    }

    #region Raise Events

    #region OnCreateVFX
    void OnCreateVFX(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[0] == "CreateVFX")
        {
            if (!_Timer.HasGameStartVFXInstantiated)
            {
                _Timer.VfxHolder.CreateVFX(0);
                _Timer.HasGameStartVFXInstantiated = true;
            }
        }
    }
    #endregion

    #region OnPlaySoundFX
    void OnPlaySoundFX(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[1] == "PlaySoundFX")
        {
            if (_Timer.Seconds <= 10) _UISoundsInGame.PlaySoundFX(0);
        }
    }
    #endregion

    #region OnPlayerActivity
    void OnPlayerActivity(ExitGames.Client.Photon.EventData obj)
    {
        object[] datas = (object[])obj.CustomData;

        if ((string)datas[5] == "PlayerActivity")
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                GameObject playerTagObj = (GameObject)player.TagObject != null ? (GameObject)player.TagObject : null;

                if (playerTagObj != null)
                {
                    IPlayerGameController playerController = playerTagObj.GetComponent<IPlayerGameController>();

                    CheckPlayerParticipationInVoting(playerController);
                    OnNightPhase(playerController);
                    OnDayPhase(playerController);
                    OnResetPhases(playerController);
                }
            }
        }
    }
    #endregion

    #region CheckPlayerParticipationInVoting
    void CheckPlayerParticipationInVoting(IPlayerGameController playerController)
    {
        if(Array.Find(_Timer.GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr) != null)
        {
            if (playerController.IsPlayerAlive != Array.Find(_Timer.GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr)._GameInfo.IsPlayerAlive)
                playerController.IsPlayerAlive = Array.Find(_Timer.GameManagerSetPlayersRoles._RoleButtonControllers.RoleButtons, item => item._OwnerInfo.OwnerActorNumber == playerController.PhotonView.OwnerActorNr)._GameInfo.IsPlayerAlive;

            if (_Timer.NightTime && _Timer.Seconds <= 30 && playerController.IsPlayerAlive)
                playerController.CanPlayerBeActiveInNightPhase = true;
            else playerController.CanPlayerBeActiveInNightPhase = false;

            if (_Timer.DayTime && _Timer.Seconds <= 60 && playerController.IsPlayerAlive)
                playerController.CanPlayerBeActiveInDayPhase = true;
            else playerController.CanPlayerBeActiveInDayPhase = false;
        }

    }
    #endregion

    #region OnNightPhase
    void OnNightPhase(IPlayerGameController playerController)
    {
        if (_Timer.NightTime && _Timer.Seconds <= 30 && playerController.CanPlayerBeActiveInNightPhase && !playerController.HasPlayerVotedInNightPhase)
        {
            playerController.HasPlayerVotedInDayPhase = false;
            playerController.HasVotePhaseResetted = false;

            if (playerController.PhotonView.IsMine)
            {
                foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                {
                    if (roleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        roleButton.GameObjectActivity(0, true);
                    }
                }
            }
        }
    }
    #endregion

    #region OnDayPhase
    void OnDayPhase(IPlayerGameController playerController)
    {
        if (_Timer.DayTime && _Timer.Seconds <= 60 && playerController.CanPlayerBeActiveInDayPhase && !playerController.HasPlayerVotedInDayPhase)
        {
            playerController.HasPlayerVotedInNightPhase = false;
            playerController.HasVotePhaseResetted = false;

            if (playerController.PhotonView.IsMine)
            {
                foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                {
                    if (roleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        roleButton.GameObjectActivity(0, true);
                    }
                }
            }
        }
    }
    #endregion

    #region OnResetPhases
    void OnResetPhases(IPlayerGameController playerController)
    {
        if (_Timer.NightTime && _Timer.Seconds > 30 || _Timer.DayTime && _Timer.Seconds > 60)
        {
            if (!playerController.HasVotePhaseResetted)
            {
                playerController.HasPlayerVotedInDayPhase = false;
                playerController.HasPlayerVotedInNightPhase = false;
                playerController.HasVotePhaseResetted = true;

                if (playerController.PhotonView.IsMine)
                {
                    foreach (var roleButton in FindObjectsOfType<RoleButtonController>())
                    {
                        if (roleButton._OwnerInfo.OwnerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                        {
                            roleButton.GameObjectActivity(0, false);
                        }
                    }
                }
            }
        }
    }
    #endregion

    #endregion
}

