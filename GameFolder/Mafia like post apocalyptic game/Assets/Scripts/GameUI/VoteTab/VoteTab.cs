using System;
using UnityEngine;
using UnityEngine.UI;

public class VoteTab : MonoBehaviour
{
    public event Action OnVoteTabOpened;
    public event Action OnVoteTabClosed;

    [Header("COMPONENTS")]
    [SerializeField] protected VoteTabBackground voteTabBackground;
    [SerializeField] protected Text _text;
    [SerializeField] protected Animator anim;

    public Text Text => _text;
    public string _Text
    {
        get
        {
            return _text.text;
        }
        set
        {
            _text.text = value;
        }
    }


    void OnEnable()
    {
        voteTabBackground.OnAnimationEnd += VoteTabBackground_OnVoteTabClosed;
    }
    
    void OnDisable()
    {
        voteTabBackground.OnAnimationEnd -= VoteTabBackground_OnVoteTabClosed;
    }

    public void PlayVoteTabAnimation(string text)
    {
        _Text = text;
        anim.SetTrigger("play");
        
        OnVoteTabOpened?.Invoke();
    }

    void VoteTabBackground_OnVoteTabClosed()
    {
        OnVoteTabClosed?.Invoke();      
    }





}
