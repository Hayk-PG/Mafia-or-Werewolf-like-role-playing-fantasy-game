using UnityEngine;

public class PlayerGameEnd : MonoBehaviour
{
    GameManagerTimer _GameManagerTimer { get; set; }
    EndTab _EndTab { get; set; }


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
        _EndTab = FindObjectOfType<EndTab>();
    }

    void Update()
    {
        if(_GameManagerTimer._Timer.IsGameFinished && !_EndTab._UI.CanvasGroup.interactable)
        {
            _EndTab.OpenEndTab();
        }
        if(!_GameManagerTimer._Timer.IsGameFinished && _EndTab._UI.CanvasGroup.interactable)
        {
            MyCanvasGroups.CanvasGroupActivity(_EndTab._UI.CanvasGroup, false);
        }
    }
}
