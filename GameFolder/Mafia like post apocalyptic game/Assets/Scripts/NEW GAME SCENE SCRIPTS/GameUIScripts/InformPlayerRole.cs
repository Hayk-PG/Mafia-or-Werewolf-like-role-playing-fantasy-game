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
        [SerializeField] Image roleImage;

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
        internal Sprite RoleImage
        {
            get => roleImage.sprite;
            set => roleImage.sprite = value;
        }
    }
    public UI _UI;


    void Update()
    {
        _UI.GotItButton.onClick.RemoveAllListeners();
        _UI.GotItButton.onClick.AddListener(() => 
        {
            MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, false);
            PlayerBaseConditions.UiSounds.PlaySoundFX(5);
        }); 
    }

    public void OnPopUp(string text, Sprite roleImage)
    {
        _UI.Text = text;
        _UI.RoleImage = roleImage;
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);        
    }

}
