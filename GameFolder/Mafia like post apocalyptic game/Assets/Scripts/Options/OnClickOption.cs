using UnityEngine;
using UnityEngine.UI;

public class OnClickOption : MonoBehaviour
{
    [SerializeField] Button optionButton;

    void Update()
    {
        OnClickOptionButton();
    }

    void OnClickOptionButton()
    {
        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(() => {
            Options.instance.OnPressedOptionButton();
            if (PlayerBaseConditions.VFXCamera() != null) PlayerBaseConditions.VFXCamera().enabled = false;

            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
        });
    }
}
