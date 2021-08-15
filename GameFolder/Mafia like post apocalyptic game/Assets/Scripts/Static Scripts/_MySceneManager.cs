using UnityEngine.SceneManagement;

public class _MySceneManager 
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static bool IsDesiredScene(string sceneName)
    {
        return SceneManager.GetActiveScene() == SceneManager.GetSceneByName(sceneName);
    }

    public static Scene CurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    public static void ChangeToMenuScene()
    {
        if (Photon.Pun.PhotonNetwork.IsConnected)
        {
            if (Photon.Pun.PhotonNetwork.InRoom) Photon.Pun.PhotonNetwork.LeaveRoom();
            if (Photon.Pun.PhotonNetwork.InLobby) Photon.Pun.PhotonNetwork.LeaveLobby();

            Photon.Pun.PhotonNetwork.LoadLevel(SceneNames.MenuScene);
        }
        else
        {
            LoadScene(SceneNames.MenuScene);
        }
    }
}
