using System;
using UnityEngine;
using UnityEngine.UI;

public class InformPlayerRole : MonoBehaviour
{
    [Serializable] public struct UI
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Text text;
        [SerializeField] Button gotItButton;

        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
        }
        public string Text
        {
            get => text.text;
            set => text.text = value;
        }
        internal Button GotItButton
        {
            get => gotItButton;
        }
    }
    public UI _UI;


    void Update()
    {
        _UI.GotItButton.onClick.RemoveAllListeners();
        _UI.GotItButton.onClick.AddListener(() => { MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, false); PlayerBaseConditions.VFXCamera().enabled = true; }); 
    }

    public void OnPopUp(string text)
    {
        _UI.Text = text;
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);
        PlayerBaseConditions.VFXCamera().enabled = false;
    }

}
