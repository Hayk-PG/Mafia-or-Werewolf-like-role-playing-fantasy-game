using System;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public static LoadingUI LI;

    [Serializable]
    public class ConnectingScreen
    {
        [SerializeField] Animator anim;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] ConnectionController _ConnectionController;

        public void EnableConnectionLostScreen()
        {
            if (!canvasGroup.interactable)
            {
                MyCanvasGroups.CanvasGroupActivity(canvasGroup, true);
                anim.enabled = true;
                anim.Play("LoadingIconAnim");
                _ConnectionController.Reconnect();
            }
        }

        public void DisableConnectionLostScreen()
        {
            if (canvasGroup.interactable)
            {
                MyCanvasGroups.CanvasGroupActivity(canvasGroup, false);
                anim.enabled = false;
            }
        }
    }
    public ConnectingScreen _ConnectingScreen;


    void Awake()
    {
        Instance();
    }

    void Instance()
    {
        if (LI == null)
        {
            LI = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
