using UnityEngine;
using UnityEngine.UI;

public class FriendButtonScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text friendNameText;
    [SerializeField] Button deleteFriendButton;
    [SerializeField] Image statusImage;

    public string Name
    {
        get => transform.name;
        set => transform.name = value;
    }
    public string FriendName
    {
        get => friendNameText.text;
        set => friendNameText.text = value;
    }  
    public Color32 StatusImageColor
    {
        get => statusImage.color;
        set => statusImage.color = value;
    }
}
