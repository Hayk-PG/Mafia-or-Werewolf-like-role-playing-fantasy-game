using System.Collections;
using UnityEngine;
using PlayFab;
using System;

public class PlayfabLogOut : MonoBehaviour
{
    public void LogOut(Action OpenTab)
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerBaseConditions.PlayerSavedData.DeleteUsernameAndPassword();
        Photon.Pun.PhotonNetwork.Disconnect();
        _MySceneManager.ChangeToMenuScene();     
        StartCoroutine(ReopenSignUpInTab(OpenTab));
    }

    IEnumerator ReopenSignUpInTab(Action OpenTab)
    {
        yield return new WaitUntil(()=> _MySceneManager.IsDesiredScene(SceneNames.MenuScene) && PlayerBaseConditions.IsNetworkManagerComponentsNotNull);
        yield return new WaitForSeconds(1);
        OpenTab();       
    }

}
