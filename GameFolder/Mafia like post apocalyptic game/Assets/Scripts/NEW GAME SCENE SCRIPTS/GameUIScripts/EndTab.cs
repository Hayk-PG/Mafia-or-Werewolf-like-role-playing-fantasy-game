using System;
using UnityEngine;

public class EndTab : MonoBehaviour
{
    [Serializable] public struct UI
    {
        [SerializeField] CanvasGroup canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
        }
    }
    public UI _UI;


    public void OpenEndTab()
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);
    }
}
