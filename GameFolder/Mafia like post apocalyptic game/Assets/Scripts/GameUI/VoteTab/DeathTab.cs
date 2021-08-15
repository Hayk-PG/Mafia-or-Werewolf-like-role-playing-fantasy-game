using System;
using UnityEngine;

public class DeathTab : VoteTab
{
    public event Action OnDeathTabOpened;
    public event Action OnDeathTabClosed;

    [SerializeField] ParticleSystem vfx;

    [Header("COMPONENTS")]
    [SerializeField] DeathTabBackground deathTabBG;
    

    void OnEnable()
    {
        deathTabBG.OnDeathVFXSActivity += DeathTabBG_OnDeathVFXSActivity;
    }
    
    void OnDisable()
    {
        deathTabBG.OnDeathVFXSActivity -= DeathTabBG_OnDeathVFXSActivity;
    }

    public void PlayDeathTabAnimation()
    {
        anim.SetTrigger("play");

        OnDeathTabOpened?.Invoke();
    }

    void DeathTabBG_OnDeathVFXSActivity(bool obj)
    {
        vfx.gameObject.SetActive(obj);

        if (!obj)
        {
            OnDeathTabClosed?.Invoke();
        }     
    }



}
