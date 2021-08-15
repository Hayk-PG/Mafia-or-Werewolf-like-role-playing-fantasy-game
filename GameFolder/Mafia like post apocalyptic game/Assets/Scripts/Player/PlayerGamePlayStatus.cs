using System.Collections.Generic;
using UnityEngine;

public class PlayerGamePlayStatus : MonoBehaviour
{
    [Header("PLAYER STATUS")]
    [SerializeField] bool canPlayerVote;
    [SerializeField] bool isPlayerStillPlaying;
    [SerializeField] bool gotSaved;
    [SerializeField] bool gotDiscovered;
    [SerializeField] bool gotCompromised;
    [SerializeField] bool gotConfused;

    [Header("VOTES")]
    [SerializeField] List<string> votedNames = new List<string>();

    #region PLAYER STATUS
    public bool CanPlayerVote
    {
        get
        {
            return canPlayerVote;
        }
        set
        {
            canPlayerVote = value;
        }
    }
    public bool IsPlayerStillPlaying
    {
        get
        {
            return isPlayerStillPlaying;
        }
        set
        {
            isPlayerStillPlaying = value;
        }
    }
    #endregion

    #region CLASSES ABILITIES

    /// <summary>
    /// Medic's ability
    /// </summary>
    public bool GotSaved
    {
        get
        {
            return gotSaved;
        }
        set
        {
            gotSaved = value;
        }
    }

    /// <summary>
    /// Sheriff's ability
    /// </summary>
    public bool GotDiscovered
    {
        get
        {
            return gotDiscovered;
        }
        set
        {
            gotDiscovered = value;
        }
    }

    /// <summary>
    /// Lizard's ability
    /// </summary>
    public bool GotCompromised
    {
        get
        {
            return gotCompromised;
        }
        set
        {
            gotCompromised = value;
        }
    }

    /// <summary>
    /// Lizard's ability
    /// </summary>
    public bool GotConfused
    {
        get
        {
            return gotConfused;
        }
        set
        {
            gotConfused = value;
        }
    }
    #endregion

    #region VOTES
    public int VotesCountThatPlayerGot
    {
        get
        {
            if(System.Array.Find(FindObjectsOfType<AvatarButtonController>(), avatar => avatar.name == GetComponent<SetPlayerInfo>().ActorNumber.ToString()) != null)
            {
                return int.Parse(System.Array.Find(FindObjectsOfType<AvatarButtonController>(), avatar => avatar.name == GetComponent<SetPlayerInfo>().ActorNumber.ToString()).PlayerVoteCountText);
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if(System.Array.Find(FindObjectsOfType<AvatarButtonController>(), avatar => avatar.name == GetComponent<SetPlayerInfo>().ActorNumber.ToString()))
            {
                System.Array.Find(FindObjectsOfType<AvatarButtonController>(), avatar => avatar.name == GetComponent<SetPlayerInfo>().ActorNumber.ToString()).PlayerVoteCountText = value.ToString();
            }
        }
    }
    public List<string> VotedNames
    {
        get
        {
            return votedNames;
        }
        set
        {
            votedNames = value;
        }
    }
    #endregion



    void Awake()
    {
        IsPlayerStillPlaying = true;
    }

    void OnEnable()
    {
        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameManager.OnDayVote += _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote += _MyGameManager_OnNightVote;
        });
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnDayVote -= _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote -= _MyGameManager_OnNightVote;
        }
    }

    void _MyGameManager_OnDayVote()
    {
        CanPlayerVote = true;

        GotSaved = false;
        GotDiscovered = false;
        GotCompromised = false;
    }

    void _MyGameManager_OnNightVote()
    {
        CanPlayerVote = true;
        GotConfused = false;
    }

    









}
