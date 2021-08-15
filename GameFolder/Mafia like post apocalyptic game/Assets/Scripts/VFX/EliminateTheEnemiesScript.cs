using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateTheEnemiesScript : MonoBehaviour
{
    [SerializeField] GameObject cleaveFireFX;
    [SerializeField] GameObject bloodDebuff;


    public void EnableCleaveFireFX()
    {
        cleaveFireFX.SetActive(true);
    }

    public void EnableBloodDebuff()
    {
        bloodDebuff.SetActive(true);
    }

}
