using System;
using UnityEngine;

public class VoteTabBackground : MonoBehaviour
{
    public event Action OnAnimationEnd;


    public void InvokeAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }
}
