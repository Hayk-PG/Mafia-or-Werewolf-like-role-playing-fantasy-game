using UnityEngine;
using UnityEngine.UI;

public class AvatarButtonController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Sprite hiddenAvatarSprite;
    [SerializeField] Image borderImage;
    [SerializeField] Text playerVoteCountText;   

    [Header("BOOL")]
    [SerializeField] bool isPlayerAllowedToSee;


    /// <summary>
    /// Button component
    /// </summary>
    public Button AvatarButton
    {
        get
        {
            return GetComponent<Button>();
        }
    }

    /// <summary>
    /// GameObject's name
    /// </summary>
    public string AvatarButtonName
    {
        get
        {
            return transform.name;
        }
        set
        {
            transform.name = value;
        }
    }

    /// <summary>
    /// Given name will be printed on the Name bar
    /// </summary>
    public string AvatarName
    {
        get
        {
            return transform.Find("NameBar").GetComponentInChildren<Text>().text;
        }
        set
        {
            transform.Find("NameBar").GetComponentInChildren<Text>().text = value;
        }
    }

    /// <summary>
    /// Avatar's sprite
    /// </summary>
    public Sprite AvatarSprite
    {
        get
        {
            return transform.Find("PlayerAvatar").GetComponent<Image>().sprite;
        }
        set
        {
            transform.Find("PlayerAvatar").GetComponent<Image>().sprite = value;
        }
    }

    /// <summary>
    /// Player character's real image,which will override the AvatarSprite, if player got exposed by the other players or lose the game. 
    /// It also overrides the AvatarSprite for the same team members only(Infected). Only them can see each other's HiddenAvatarSprite
    /// </summary>
    public Sprite HiddenAvatarSprite
    {
        get
        {
            return hiddenAvatarSprite;
        }
        set
        {
            hiddenAvatarSprite = value;
        }
    }

    public Image BorderImage
    {
        get => borderImage;
    }

    public string PlayerVoteCountText
    {
        get
        {
            return playerVoteCountText.text;
        }
        set
        {
            playerVoteCountText.text = value;
        }
    }

    CanvasGroup VoteTextCanvasGroup
    {
        get
        {
            return playerVoteCountText.GetComponent<CanvasGroup>();
        }
    }

    public bool IsPlayerAllowedToSee
    {
        get
        {
            return isPlayerAllowedToSee;
        }
        set
        {
            isPlayerAllowedToSee = value;
        }
    }

    public VoteBarController VoteBar
    {
        get
        {
            return transform.Find("VoteBar").GetComponent<VoteBarController>();
        }
    }

    public CanvasGroup LostTextObj
    {
        get
        {
            return transform.Find("NameBar").Find("LostText").GetComponent<CanvasGroup>();
        }
    }
  


    void Awake()
    {
        AvatarName = "";
        PlayerVoteCountText = 0.ToString();
        IsPlayerAllowedToSee = true;
    }

    void Update()
    {
        VoteTextCanvasGroupActivity();

        if (PlayerBaseConditions._HasRoundBeenChanged)
        {
            VoteBarController("", false);
        }
    }

    void VoteTextCanvasGroupActivity()
    {
        if (PlayerVoteCountText != null)
        {
            if (int.Parse(PlayerVoteCountText) <= 0)
            {
                VoteTextCanvasGroup.alpha = 0;
            }
            else
            {
                if (IsPlayerAllowedToSee)
                {
                    VoteTextCanvasGroup.alpha = 1;
                }
                else
                {
                    VoteTextCanvasGroup.alpha = 0;
                }
            }
        }
        else
        {
            VoteTextCanvasGroup.alpha = 0;
        }
    }

    public void VoteBarController(string playerName, bool isActive)
    {
        //VoteBar.Name = playerName;

        //if (isActive)
        //{
        //    VoteBar.VoteBarCanvasGroup.alpha = 1;
        //    VoteBar.VoteBarCanvasGroup.blocksRaycasts = true;
        //    VoteBar.VoteBarCanvasGroup.interactable = true;
        //    VoteBar.Anim.SetTrigger("play");           
        //}
        //else
        //{
        //    VoteBar.VoteBarCanvasGroup.alpha = 0;
        //    VoteBar.VoteBarCanvasGroup.blocksRaycasts = false;
        //    VoteBar.VoteBarCanvasGroup.interactable = false;
        //}
    }

    public void ShowHiddenAvatarSprite()
    {
        AvatarSprite = HiddenAvatarSprite;
    }

    #region AvatarColor
    public void AvatarColor(Color color)
    {
        transform.Find("NameBar").GetComponentInChildren<Text>().color = color;
        BorderImage.color = color;
    }
    #endregion

}
