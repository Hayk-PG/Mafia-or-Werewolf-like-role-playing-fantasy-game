using UnityEngine;

public class UISoundsInGame : UISounds
{
    [Header("IN GAME SOUND FX")]
    [SerializeField] AudioClip timerSoundFX;
    [SerializeField] AudioClip timerTickerSoundFX;
    [SerializeField] AudioClip dayVotePopUpSoundFX;
    [SerializeField] AudioClip deathTabPopUpSoundFX;
    [SerializeField] AudioClip onClickEmptyAvatarFX;
    [SerializeField] AudioClip dayVoteSoundFX;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions.MyComponents.PlayerSelfTimer.OnUpdateTimerText += PlayerSelfTimer_OnUpdateTimerText;
            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabOpened += VoteTab_OnVoteTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabOpened += DeathTab_OnDeathTabOpened;

            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickEmptyAvatar += GetGameManagerEvents_OnClickEmptyAvatar;
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickDayVote += GetGameManagerEvents_OnClickDayVote;
        });
    }
    
    protected override void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameControllerComponentesNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabOpened -= VoteTab_OnVoteTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabOpened -= DeathTab_OnDeathTabOpened;
        }

        if(PlayerBaseConditions.MyComponents != null)
        {
            PlayerBaseConditions.MyComponents.PlayerSelfTimer.OnUpdateTimerText -= PlayerSelfTimer_OnUpdateTimerText;
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickEmptyAvatar -= GetGameManagerEvents_OnClickEmptyAvatar;
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickDayVote -= GetGameManagerEvents_OnClickDayVote;
        }
    }

    protected override void Update()
    {
        
    }

    #region PlayerSelfTimer_OnUpdateTimerText
    void PlayerSelfTimer_OnUpdateTimerText(byte obj)
    {
        PlayUISoundFX(PlayerBaseConditions.IsVotesLastSeconds && PlayerBaseConditions._LocalPlayerTagObject.GetComponent<PlayerGamePlayStatus>().IsPlayerStillPlaying ? timerTickerSoundFX: timerSoundFX);
    }
    #endregion

    #region VoteTab_OnVoteTabOpened
    private void VoteTab_OnVoteTabOpened()
    {
        PlayUISoundFX(dayVotePopUpSoundFX);
    }
    #endregion

    #region DeathTab_OnDeathTabOpened
    void DeathTab_OnDeathTabOpened()
    {
        PlayUISoundFX(deathTabPopUpSoundFX);
    }
    #endregion

    #region GetGameManagerEvents_OnClickEmptyAvatar
    void GetGameManagerEvents_OnClickEmptyAvatar()
    {
        PlayUISoundFX(onClickEmptyAvatarFX);
    }
    #endregion

    #region GetGameManagerEvents_OnClickDayVote
    void GetGameManagerEvents_OnClickDayVote(int obj)
    {
        PlayUISoundFX(dayVoteSoundFX);
    }
    #endregion
}
