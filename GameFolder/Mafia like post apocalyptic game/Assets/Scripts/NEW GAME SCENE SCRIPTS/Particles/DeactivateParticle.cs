using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateParticle : MonoBehaviour
{
    [SerializeField] float time;

    void OnEnable()
    {
        Invoke("OnDeactivate", time);
    } 

    void OnDeactivate()
    {
        gameObject.SetActive(false);
    }
}
