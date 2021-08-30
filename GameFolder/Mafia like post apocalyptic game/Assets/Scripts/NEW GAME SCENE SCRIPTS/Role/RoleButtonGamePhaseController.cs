using Photon.Pun;
using System;
using UnityEngine;

public class RoleButtonGamePhaseController : MonoBehaviourPun,IRoleButtonGamePhaseController
{    
    [Serializable] class GameObjects
    {
        [SerializeField] GameObject aimObj;

        internal GameObject Aim
        {
            get => aimObj;
        }
    }
    [SerializeField] GameObjects _GameObjects;


    public RoleButtonController RoleButtonController { get; set; }


    void Awake()
    {
        RoleButtonController = GetComponent<RoleButtonController>();
    }  

    public void ActivateAimObj(bool isActivated)
    {
        _GameObjects.Aim.SetActive(isActivated);
    }
}
