using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System;

public class OnClickMainMenu : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;


    void Update()
    {
        OnClickMainMenuButton();
    }

    void OnClickMainMenuButton()
    {
        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() => { BackToMainMenu(); });
    }

    void BackToMainMenu()
    {
        if (_MySceneManager.CurrentScene().name != SceneNames.MenuScene)
        {
            _MySceneManager.ChangeToMenuScene();
            Options.instance.OnPressedOptionsButtons();
            StartCoroutine(ReopenCreateOrFindTab(()=> PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnBackToMainMenu()));
        }
    }

    IEnumerator ReopenCreateOrFindTab(Action OpenTab)
    {
        yield return new WaitUntil(() => _MySceneManager.IsDesiredScene(SceneNames.MenuScene) && PlayerBaseConditions.IsNetworkManagerComponentsNotNull);
        yield return new WaitForSeconds(1);
        OpenTab();
    }
}
