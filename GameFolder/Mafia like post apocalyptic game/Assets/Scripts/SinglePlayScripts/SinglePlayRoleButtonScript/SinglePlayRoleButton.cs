using System;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayRoleButton : MonoBehaviour
{
    [Serializable] public class Info
    {
        [SerializeField] internal Text nameText;
        [SerializeField] internal Text roleNameText;
        [SerializeField] internal Text votesCountText;
        [SerializeField] internal Text votedPlayerNameText;

        [SerializeField] internal Image roleImage;
        [SerializeField] internal Sprite actualRoleSprite;

        [SerializeField] internal string roleName;

        [SerializeField] internal bool isPlayer;
        [SerializeField] internal bool isAlive;
        [SerializeField] internal bool hasVoted;
        [SerializeField] internal bool isHealed;
        [SerializeField] internal bool isRevealed;
        [SerializeField] internal bool isKilledByKnight;
        [SerializeField] internal bool isConjured;

        internal GameObject VotesCountObj
        {
            get => votesCountText.gameObject;
        }
    }    
    [Serializable] public class UI
    {
        [SerializeField] Button roleButton;

        [SerializeField] Image voteIcon;        

        [SerializeField] GameObject selected;
        [SerializeField] GameObject votesInfoObj;
        [SerializeField] GameObject deathIconsObj;

        [SerializeField] Sprite[] voteIconsPrefab;

        public Button RoleButton
        {
            get => roleButton;
        }
        
        public GameObject VoteIconObj
        {
            get => voteIcon.gameObject;
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

        public Sprite VoteIcon
        {
            get => voteIcon.sprite;
            set => voteIcon.sprite = value;
        }
        public Sprite[] VoteIconsPrefab
        {
            get => voteIconsPrefab;
        }
    }
    [Serializable] public class VFX
    {
        [SerializeField] GameObject deathVFX;
        [SerializeField] GameObject[] abilityVFX;

        public GameObject DeathVFX
        {
            get => deathVFX;
        }
        public GameObject[] AbilityVFX
        {
            get => abilityVFX;
        }
    }

    public Info _Info;
    public UI _UI;
    public VFX _VFX;

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
    public string RoleNameText
    {
        get => _Info.roleNameText.text;
        set => _Info.roleNameText.text = value;
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
    public bool IsHealed
    {
        get => _Info.isHealed;
        set => _Info.isHealed = value;
    }
    public bool IsKilledByKnight
    {
        get => _Info.isKilledByKnight;
        set => _Info.isKilledByKnight = value;
    }
    public bool IsConjured
    {
        get => _Info.isConjured;
        set => _Info.isConjured = value;
    }

    CardsHolder _CardsHolder { get; set; }
    SinglePlayGameController _SinglePlayGameController { get; set; }
    SinglePlayVoteDatas _SinglePlayVoteDatas { get; set; }
    SinglePlayerPoints _SinglePlayerPoints { get; set; }


    void Awake()
    {
        _CardsHolder = FindObjectOfType<CardsHolder>();
        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
        _SinglePlayVoteDatas = FindObjectOfType<SinglePlayVoteDatas>();
        _SinglePlayerPoints = FindObjectOfType<SinglePlayerPoints>();
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

    #region PlayerDayVote
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
            roleButtons.VotesIconActivity(false);
        });

        SelectedIconActivty(true);
        PlayerDayVoteAction();

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

    void PlayerDayVoteAction()
    {
        if (SinglePlayGlobalConditions.AmIKing()) VotesCount += 2;
        else AddVotesCount();

        _SinglePlayerPoints.PointsForDayVote(new SinglePlayerPoints.Data(_SinglePlayGameController.PlayerRoleButton(), this, 10, -15));
        _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayGameController.PlayerRoleButton(), _SinglePlayGameController._TimerClass.DaysCount, this);
    }
    #endregion

    #region PlayerNightVote
    void PlayerNightVote()
    {
        if (SinglePlayGlobalConditions.CanParticipateInNightVote() && SinglePlayGlobalConditions.IsVoteTime() && _SinglePlayGameController._TimerClass.IsNight)
        {
            if (!IsPlayer && IsAlive)
            {
                PlayerNightVoted();
            }
        }
        // _CardsHolder._Conditions.Maximize = true;
    }

    void PlayerNightVoted()
    {
        _SinglePlayGameController.PlayerRoleButton().HasVotedCondition(true);
        _SinglePlayGameController.PlayerRoleButton().DisplayVotesInfo(SinglePlayGlobalConditions.AmIInfected(), Name);
        _SinglePlayGameController.LoopRoleButtons(roleButtons =>
        {
            roleButtons.VotesIconActivity(false);
        });

        SelectedIconActivty(true);
        PlayerNightVoteAction();

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

    void PlayerNightVoteAction()
    {
        if (SinglePlayGlobalConditions.AmIMedic())
        {
            IsHealed = true;
            ObjActivity(_VFX.AbilityVFX[0], true);
            _SinglePlayerPoints.PointsForMedic(new SinglePlayerPoints.Data(null, this, 30, 0));
        }
        if (SinglePlayGlobalConditions.AmISheriff())
        {
            IsRevealed = true;
            RoleNameText = RoleName;
            ObjActivity(_VFX.AbilityVFX[1], true);
            RoleImage = RoleSprite;
            _SinglePlayerPoints.PointsForSheriff(new SinglePlayerPoints.Data(null, this, 20, 0));
        }
        if (SinglePlayGlobalConditions.AmISoldier())
        {
            IsKilledByKnight = true;
            ObjActivity(_VFX.AbilityVFX[2], true);
            _SinglePlayerPoints.PointsForSoldier(new SinglePlayerPoints.Data(null, this, 25, -25));
        }
        if (SinglePlayGlobalConditions.AmIInfected())
        {
            AddVotesCount();
            ObjActivity(_VFX.AbilityVFX[3], true);
            _SinglePlayerPoints.PointsForInfected(new SinglePlayerPoints.Data(null, this, 25, -25));
        }
        if (SinglePlayGlobalConditions.AmILizard())
        {
            IsConjured = true;
            ObjActivity(_VFX.AbilityVFX[4], true);
            _SinglePlayerPoints.PointsForLizard(new SinglePlayerPoints.Data(null, this, 30, -5));
        }
    }

    void ResetAbilityVFX()
    {
        foreach (var vfx in _VFX.AbilityVFX)
        {
            ObjActivity(vfx, false);
        }
    }
    #endregion

    #region Reset
    void _SinglePlayGameController_OnPhaseReset(bool isNightTime)
    {
        HasVotedCondition(false);
        VotesIconActivity(false);
        ResetVotesIcon();
        ResetVotes();
        VotesCountTextObjActivity(false);
        SelectedIconActivty(false);
        DisplayVotesInfo(false, "");
        ResetAffects(isNightTime);
        ResetAbilityVFX();
    }

    void ResetAffects(bool isNightTime)
    {
        if (isNightTime)
        {
            IsHealed = false;
            IsConjured = false;
        }
    }
    #endregion

    #region Votes
    public void AddVotesCount()
    {        
        VotesCount++;
    }

    void ResetVotes()
    {
        VotesCount = 0;
    }

    public void VotesCountTextObjActivity(bool isActive)
    {
        if (IsAlive)
        {
            if (_Info.VotesCountObj.activeInHierarchy != isActive) _Info.VotesCountObj.SetActive(isActive);
        }
    }

    public void DisplayVotesInfo(bool isActive, string votedPlayerName)
    {
        if(VotedPlayerName != votedPlayerName) VotedPlayerName = votedPlayerName;
        if(_UI.VotesInfoObj.activeInHierarchy != isActive) _UI.VotesInfoObj.SetActive(isActive);
    }

    public void SelectedIconActivty(bool isActive)
    {
        ObjActivity(_UI.Selected, isActive);
    }

    public void HasVotedCondition(bool hasVoted)
    {
        HasVoted = hasVoted;
    }
    #endregion

    #region Lost
    public void Lost()
    {       
        IsAlive = false;
        RoleNameText = RoleName;
        ObjActivity(_VFX.DeathVFX, true);
        _UI.DeathIconsObj.SetActive(true);

        if (!IsPlayer)
        {
            RoleImage = RoleSprite;
        }
    }
    #endregion

    #region VotesIcon
    public void VotesIconActivity(bool activity)
    {       
        if (!IsPlayer && IsAlive)
        {
            ObjActivity(_UI.VoteIconObj, activity);

            if (_SinglePlayGameController._TimerClass.IsNight)
            {
                if (_UI.VoteIcon != _UI.VoteIconsPrefab[VoteIconIndex()]) _UI.VoteIcon = _UI.VoteIconsPrefab[VoteIconIndex()];
            }
            else
            {
                if (_UI.VoteIcon != _UI.VoteIconsPrefab[0]) _UI.VoteIcon = _UI.VoteIconsPrefab[0];
            }
        }
    }

    int VoteIconIndex()
    {
        return SinglePlayGlobalConditions.AmIMedic() ? 1 :
               SinglePlayGlobalConditions.AmISheriff() ? 2 :
               SinglePlayGlobalConditions.AmISoldier() ? 3 :
               SinglePlayGlobalConditions.AmIInfected() ? 4 :
               SinglePlayGlobalConditions.AmILizard() ? 5 : 0;
    }

    public void ResetVotesIcon()
    {
        ObjActivity(_UI.VoteIconObj, false);
    }

    #endregion

    #region Abilities
    /// <summary>
    /// 0: Medic 1: Sheriff 2: Knight 3: Infected 4: Lizard
    /// </summary>
    /// <param name="roleIndex"></param>
    public void AIAbility(int roleIndex)
    {
        switch (roleIndex)
        {
            case 0:
                IsHealed = true;
                if(IsPlayer) ObjActivity(_VFX.AbilityVFX[0], true);
                break;

            case 1:
                IsRevealed = true;
                if (IsPlayer) ObjActivity(_VFX.AbilityVFX[1], true);
                break;

            case 2:
                IsKilledByKnight = true;
                if (IsPlayer) ObjActivity(_VFX.AbilityVFX[2], true);
                break;

            case 3:
                AddVotesCount();
                if (IsPlayer) ObjActivity(_VFX.AbilityVFX[3], true);
                break;

            case 4:
                IsConjured = true;
                if (IsPlayer) ObjActivity(_VFX.AbilityVFX[4], true);
                break;
        }
    }
    #endregion

    public void ObjActivity(GameObject obj, bool enabled)
    {
        if (obj.activeInHierarchy != enabled) obj.SetActive(enabled);
    }
}
