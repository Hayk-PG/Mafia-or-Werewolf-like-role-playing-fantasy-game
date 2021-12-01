using UnityEngine;
using UnityEngine.UI;

public abstract class OptionButtonsBaseScript : MonoBehaviour
{
    [SerializeField] protected Button button;

    protected void Update()
    {
        OnClickButton();
    }
    protected void OnClickButton()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { PlayClickSound(button.name); Logic();});
    }

    protected void PlayClickSound(string buttonName)
    {
        PlayerBaseConditions.UiSounds.PlaySoundFX(buttonName == "ResumeButton" ? 1 : 0);
    }

    protected abstract void Logic();   
}
