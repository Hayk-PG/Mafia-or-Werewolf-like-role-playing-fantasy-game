﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabIsLoggedIn : MonoBehaviour
{  
    public bool IsPlayfabLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }
}
