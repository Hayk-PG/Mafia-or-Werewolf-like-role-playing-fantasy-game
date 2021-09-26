using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWallVolcanicScript : MonoBehaviour
{
    [Header("PARTICLE")]
    [SerializeField] ParticleSystem endDebris;

    bool isEndDebrisActive;


    void Update()
    {
        if(endDebris.particleCount > 0  && !isEndDebrisActive)
        {           
            isEndDebrisActive = true;
            FindObjectOfType<UISoundsInGame>().PlaySoundFXinGame(4);
        }
        if(endDebris.particleCount <= 0)
        {
            isEndDebrisActive = false;
        }
    }
}
