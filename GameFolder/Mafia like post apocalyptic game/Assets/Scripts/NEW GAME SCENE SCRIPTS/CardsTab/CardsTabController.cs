using System;
using UnityEngine;
using UnityEngine.UI;

public class CardsTabController : MonoBehaviour
{
    [Serializable] class Death
    {
        [SerializeField] CanvasGroup deathTabCanvasGroup;
        [SerializeField] Text nameText;
        [SerializeField] Animator deathCardAnim;
        [SerializeField] ParticleSystem FireEnchant;
        [SerializeField] ParticleSystem GodRaysSharp;
        [SerializeField] ParticleSystem SpikeVolcanicWall;

        internal string DeathPlayerName
        {
            get => nameText.text;
            set => nameText.text = value;
        }

        internal void OpenDeathTab()
        {
            MyCanvasGroups.CanvasGroupActivity(deathTabCanvasGroup, true);
            deathCardAnim.SetTrigger("play");
            FireEnchant.Play();
            GodRaysSharp.Play();
            SpikeVolcanicWall.Play();
        }
        internal void CloseDeathTab()
        {
            MyCanvasGroups.CanvasGroupActivity(deathTabCanvasGroup, false);
            FireEnchant.Stop();
            GodRaysSharp.Stop();
        }
    }

    [SerializeField] Death _Death;

    [SerializeField] CanvasGroup cardsTabCanvasGroup;
    public CanvasGroup CardsTabCanvasGroup
    {
        get => cardsTabCanvasGroup;
    }


    public void OnDeathTab(bool isActive, string deathPlayerName)
    {
        if(CardsTabCanvasGroup.interactable != isActive)
        {
            MyCanvasGroups.CanvasGroupActivity(CardsTabCanvasGroup, isActive);
            _Death.DeathPlayerName = deathPlayerName;

            if (isActive)
            {
                _Death.OpenDeathTab();
                UISoundsInGame Sounds = FindObjectOfType<UISoundsInGame>();
                Sounds.PlaySoundFXinGame(2);
                Sounds.PlaySoundFXinGame(3);
            }
            else
            {
                _Death.CloseDeathTab();
            }
        } 
    }
}
