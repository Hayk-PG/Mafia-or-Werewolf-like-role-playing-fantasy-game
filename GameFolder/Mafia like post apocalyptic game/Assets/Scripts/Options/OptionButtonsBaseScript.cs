using UnityEngine;
using UnityEngine.UI;

public abstract class OptionButtonsBaseScript : MonoBehaviour
{
    [SerializeField] protected Button[] buttons;
    protected abstract Button[] Buttons { get;}




    protected abstract void Update();
    protected abstract void OnClickButton();
}
