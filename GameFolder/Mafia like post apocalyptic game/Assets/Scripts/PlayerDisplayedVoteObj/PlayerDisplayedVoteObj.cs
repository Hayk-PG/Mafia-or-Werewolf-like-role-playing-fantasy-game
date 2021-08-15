using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplayedVoteObj : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text nameText;

    public string Name
    {
        get => nameText.text;
        set => nameText.text = value;
    }
}
