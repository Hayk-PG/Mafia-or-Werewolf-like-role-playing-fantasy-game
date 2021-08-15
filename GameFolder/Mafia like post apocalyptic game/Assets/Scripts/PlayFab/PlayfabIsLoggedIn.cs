using UnityEngine;
using PlayFab;


public class PlayfabIsLoggedIn : MonoBehaviour
{  
    public bool IsPlayfabLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }
}
