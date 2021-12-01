using UnityEngine;

public class NightStarsScript : MonoBehaviour
{
    [SerializeField] GameObject starsParticles;

    GameObject StarsParticle
    {
        get => starsParticles;
    }
    SinglePlayGameController _SinglePlayGameController;


    void Awake()
    {
        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
    }

    void OnEnable()
    {
        _SinglePlayGameController.OnPhaseReset += OnNightStars;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnPhaseReset -= OnNightStars;
    }

    void OnNightStars(bool isNight)
    {
        if (isNight && StarsParticle.activeInHierarchy != true)
            StarsParticle.SetActive(true);
        else if(!isNight && StarsParticle.activeInHierarchy != false)
            StarsParticle.SetActive(false);
    }

}
