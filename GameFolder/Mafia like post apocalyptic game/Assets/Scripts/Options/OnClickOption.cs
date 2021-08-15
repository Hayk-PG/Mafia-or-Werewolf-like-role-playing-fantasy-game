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
        optionButton.onClick.AddListener(() => { Options.instance.OnPressedOptionButton(); });
    }
}
