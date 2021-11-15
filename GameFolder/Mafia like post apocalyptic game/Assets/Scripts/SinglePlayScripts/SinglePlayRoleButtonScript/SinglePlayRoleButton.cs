using System;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayRoleButton : MonoBehaviour
{
    [Serializable] public class Info
    {
        [SerializeField] internal Text nameText;
        [SerializeField] internal Text votesCountText;
        [SerializeField] internal Text votedPlayerNameText;
        [SerializeField] internal Image roleImage;
        [SerializeField] internal Image votesCountSphereImage;
        [SerializeField] internal Sprite actualRoleSprite;
        [SerializeField] internal string roleName;
        [SerializeField] internal bool isPlayer;
        [SerializeField] internal bool isAlive;
        [SerializeField] internal bool hasVoted;
        [SerializeField] internal bool isRevealed;
    }
    [Serializable] public class Vfx
    {
        [SerializeField] GameObject[] voteVFX;
        [SerializeField] GameObject[] voteVFXExplosion;
        [SerializeField] GameObject deathVFX;

        public GameObject[] VoteVFX
        {
            get => voteVFX;
        }
        public GameObject[] VoteVFXExplosion
        {
            get => voteVFXExplosion;
        }
        public GameObject DeathVFX
        {
            get => deathVFX;
        }
    }   
    [Serializable] public class UI
    {
        [SerializeField] Button roleButton;
        [SerializeField] GameObject selected;
        [SerializeField] GameObject votesInfoObj;
        [SerializeField] GameObject deathIconsObj;

        public Button RoleButton
        {
            get => roleButton;
        }
        public GameObject Selected
        {
            get => selected;
        }
        public GameObject VotesInfoObj
        {
            get => votesInfoObj;
        }
        public GameObject DeathIconsObj
        {
            get => deathIconsObj;
        }
    }
    [Serializable] public class Components
    {
        [SerializeField] Animator votesCountSphereAnim;

        public Animator VotesCountSphereAnim
        {
            get => votesCountSphereAnim;
        }
    }

    public Info _Info;
    public Vfx _Vfx;
    public UI _UI;
    public Components _Components;

    public string Name
    {
        get => _Info.nameText.text;
        set => _Info.nameText.text = value;
    }
    public string RoleName
    {
        get => _Info.roleName;
        set => _Info.roleName = value;
    }
    public string VotedPlayerName
    {
        get => _Info.votedPlayerNameText.text;
        set => _Info.votedPlayerNameText.text = value;
    }
    public int VotesCount
    {
        get => int.Parse(_Info.votesCountText.text);
        set => _Info.votesCountText.text = value.ToString();
    }
    public Sprite RoleSprite
    {
        get => _Info.actualRoleSprite;
        set => _Info.actualRoleSprite = value;
    }
    public Sprite RoleImage
    {
        get => _Info.roleImage.sprite;
        set => _Info.roleImage.sprite = value;
    }
    public bool IsPlayer
    {
        get => _Info.isPlayer;
        set => _Info.isPlayer = value;
    }
    public bool IsAlive
    {
        get => _Info.isAlive;
        set => _Info.isAlive = value;
    }
    public bool HasVoted
    {
        get => _Info.hasVoted;
        set => _Info.hasVoted = value;
    }
    public bool IsRevealed
    {
        get => _Info.isRevealed;
        set => _Info.isRevealed = value;
    }

    CardsHolder _CardsHolder { get; set; }
    SinglePlayGameController _SinglePlayGameController { get; set; }
    SinglePlayVoteDatas _SinglePlayVoteDatas { get; set; }


    void Awake()
    {
        _CardsHolder = FindObjectOfType<CardsHolder>();
        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
        _SinglePlayVoteDatas = FindObjectOfType<SinglePlayVoteDatas>();
    }

    void OnEnable()
    {
        _SinglePlayGameController.OnPhaseReset += _SinglePlayGameController_OnPhaseReset;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnPhaseReset -= _SinglePlayGameController_OnPhaseReset;
    }

    void Update()
    {
        OnClickRoleButton();
    } 
  
    public void OnClickRoleButton()
    {
        _UI.RoleButton.onClick.RemoveAllListeners();
        _UI.RoleButton.onClick.AddListener(delegate { PlayerDayVote(); PlayerNightVote(); });
    }

    void PlayerDayVote()
    {
        if (SinglePlayGlobalConditions.CanParticipateInDayVote() && SinglePlayGlobalConditions.IsVoteTime() && !_SinglePlayGameController._TimerClass.IsNight)
        {
            if (!IsPlayer && IsAlive)
            {
                PlayerDayVoted();
            }
        }
    }

    void PlayerDayVoted()
    {
        _SinglePlayGameController.PlayerRoleButton().HasVotedCondition(true);
        _SinglePlayGameController.PlayerRoleButton().DisplayVotesInfo(true, Name);
        _SinglePlayGameController.LoopRoleButtons(roleButtons =>
        {
            roleButtons.VotesIndicatorVfxActivity(0, false);
        });

        VotesIndicatorVfxExplosionActivty(0, true);
        SelectedIconActivty(true);
        AddVotesCount();    

        if (SinglePlayGlobalConditions.AmIInfected())
        {
            _SinglePlayVoteDatas.AddInfectedsDayVotesInfo
            (
            _SinglePlayGameController.PlayerRoleButton().RoleName,
            new SinglePlayVoteDatas.DayVotesInfo
            (Name,
            _SinglePlayGameController._TimerClass.DaysCount
            ));
        }
    }

    void PlayerNightVote()
    {
        //_CardsHolder._Conditions.Maximize = true;
    }
 
    void _SinglePlayGameController_OnPhaseReset(bool isNightTime)
    {
        HasVotedCondition(false);
        VotesIndicatorVfxActivity(0, false);
        VotesIndicatorVfxExplosionActivty(0, false);
        ResetVotes();
        VotesCountTextObjActivity(false);
        SelectedIconActivty(false);
        DisplayVotesInfo(false, "");
    }

    #region Votes
    public void AddVotesCount()
    {        
        VotesCount++;
        _Components.VotesCountSphereAnim.SetTrigger("play");
    }

    void ResetVotes()
    {
        VotesCount = 0;
    }

    public void VotesCountTextObjActivity(bool isActive)
    {
        if (IsAlive)
        {
            if (_Info.votesCountSphereImage.gameObject.activeInHierarchy != isActive) _Info.votesCountSphereImage.gameObject.SetActive(isActive);
        }
    }

    public void DisplayVotesInfo(bool isActive, string votedPlayerName)
    {
        if(VotedPlayerName != votedPlayerName) VotedPlayerName = votedPlayerName;
        if(_UI.VotesInfoObj.activeInHierarchy != isActive) _UI.VotesInfoObj.SetActive(isActive);
    }

    public void SelectedIconActivty(bool isActive)
    {
        if (_UI.Selected.activeInHierarchy != isActive) _UI.Selected.SetActive(isActive);
    }

    public void HasVotedCondition(bool hasVoted)
    {
        HasVoted = hasVoted;
    }
    #endregion

    #region Lost
    public void Lost()
    {
        _Vfx.DeathVFX.SetActive(true);
        IsAlive = false;
        _UI.DeathIconsObj.SetActive(true);

        if (!IsPlayer)
        {
            RoleImage = RoleSprite;
        }
    }
    #endregion

    #region VFX
    public void VotesIndicatorVfxActivity(int vfxIndex, bool isActive)
    {
        if (!IsPlayer && IsAlive)
        {
            if (_Vfx.VoteVFX[vfxIndex].activeInHierarchy != isActive) _Vfx.VoteVFX[vfxIndex].SetActive(isActive);
        }
    }

    public void VotesIndicatorVfxExplosionActivty(int vfxIndex, bool isActive)
    {
        if (!IsPlayer)
        {
            if (_Vfx.VoteVFXExplosion[vfxIndex].activeInHierarchy != isActive) _Vfx.VoteVFXExplosion[vfxIndex].SetActive(isActive);
        }
    }
    #endregion
}
