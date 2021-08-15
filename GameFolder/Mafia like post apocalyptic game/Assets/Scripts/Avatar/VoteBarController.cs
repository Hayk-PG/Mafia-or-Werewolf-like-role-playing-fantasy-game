using UnityEngine;
using UnityEngine.UI;

public class VoteBarController : MonoBehaviour
{
    Text NameText
    {
        get
        {
            return GetComponentInChildren<Text>();
        }
    }
    public string Name
    {
        get
        {
            return NameText.text;
        }
        set
        {
            NameText.text = value;
        }
    }
    public CanvasGroup VoteBarCanvasGroup
    {
        get
        {
            return GetComponent<CanvasGroup>();
        }
    }
    public Animator Anim
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    public AudioClip VoteBarPopUpSoundFX { get; set; }

    bool isVoteBarPopUpSoundFXPlayed;


    void Awake()
    {
        name = "VoteBar";
        VoteBarPopUpSoundFX = Resources.Load<AudioClip>("VoteBarPopUpSoundFX/Pop sounds 17 (VoteBarPopUpSoundFX)");
    }

    void Update()
    {
        PlayPopUpSoundFX();
    }

    #region PlayPopUpSoundFX
    void PlayPopUpSoundFX()
    {
        if(VoteBarCanvasGroup.alpha > 0)
        {
            if (!isVoteBarPopUpSoundFXPlayed)
            {
                PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(VoteBarPopUpSoundFX);
                isVoteBarPopUpSoundFXPlayed = true;
            }           
        }
        else
        {
            if (isVoteBarPopUpSoundFXPlayed)
            {
                isVoteBarPopUpSoundFXPlayed = false;
            }
        }
    }
    #endregion

}
