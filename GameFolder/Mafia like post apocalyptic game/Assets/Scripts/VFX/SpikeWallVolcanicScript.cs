using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWallVolcanicScript : MonoBehaviour
{
    [Header("AUDIO CLIP")]
    [SerializeField] AudioClip endDebrisSoundFX;

    [Header("PARTICLE")]
    [SerializeField] ParticleSystem endDebris;

    bool isEndDebrisActive;



    void Update()
    {
        if(endDebris.particleCount > 0  && !isEndDebrisActive)
        {
            PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(endDebrisSoundFX);
            isEndDebrisActive = true;
        }
    }


}
