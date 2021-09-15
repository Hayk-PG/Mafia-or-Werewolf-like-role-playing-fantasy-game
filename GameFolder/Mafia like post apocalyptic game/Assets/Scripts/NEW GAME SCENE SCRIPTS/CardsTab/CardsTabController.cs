using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsTabController : MonoBehaviour
{
    [Serializable] class Death
    {
        [SerializeField] CanvasGroup deathTabCanvasGroup;
        [SerializeField] Animator deathCardAnim;
        [SerializeField] ParticleSystem FireEnchant;
        [SerializeField] ParticleSystem GodRaysSharp;

        internal void OpenDeathTab()
        {
            MyCanvasGroups.CanvasGroupActivity(deathTabCanvasGroup, true);
            deathCardAnim.SetTrigger("play");
            FireEnchant.Play();
            GodRaysSharp.Play();
        }
        internal void CloseDeathTab()
        {
            MyCanvasGroups.CanvasGroupActivity(deathTabCanvasGroup, false);
            FireEnchant.Stop();
            GodRaysSharp.Stop();
        }
    }

    [SerializeField] Death _Death;


    public void OnDeathTab(bool isActive)
    {
        if(GetComponent<CanvasGroup>().interactable != isActive)
        {
            MyCanvasGroups.CanvasGroupActivity(GetComponent<CanvasGroup>(), isActive);

            if (isActive) _Death.OpenDeathTab(); else _Death.CloseDeathTab();
        } 
    }
}
