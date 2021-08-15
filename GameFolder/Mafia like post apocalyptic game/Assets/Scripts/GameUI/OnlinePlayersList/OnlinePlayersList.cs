using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayersList : MonoBehaviour
{
    [Header("CANVAS GROUP")]
    [SerializeField] CanvasGroup gameStartCanvasGroup;

    [Header("UI")]
    [SerializeField] Text onlinePlayersListText;

    public string OnlinePlayersName
    {
        get
        {
            return onlinePlayersListText.text;
        }
        set
        {
            onlinePlayersListText.text = value;
        }
    }
    public CanvasGroup GameStartCanvasGroup => gameStartCanvasGroup;














}
