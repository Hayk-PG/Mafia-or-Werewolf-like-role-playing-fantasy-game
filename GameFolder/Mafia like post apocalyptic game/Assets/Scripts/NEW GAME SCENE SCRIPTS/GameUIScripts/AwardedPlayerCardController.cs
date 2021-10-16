using System;
using UnityEngine;
using UnityEngine.UI;

public class AwardedPlayerCardController : MonoBehaviour
{
    [Serializable] public class UI
    {
        [SerializeField] Image profilePic;
        [SerializeField] Image starsImage;
        [SerializeField] Text titleText;
        [SerializeField] Text awardedText;        
        [SerializeField] internal Sprite[] starsSprites;

        internal Sprite Picture
        {
            get => profilePic.sprite;
            set => profilePic.sprite = value;
        }
        internal Sprite Stars
        {
            get => starsImage.sprite;
            set => starsImage.sprite = value;
        }
        internal string Title
        {
            get => titleText.text;
            set => titleText.text = value;
        }
        internal string Award
        {
            get => awardedText.text;
            set => awardedText.text = value;
        }
    }

    public UI _UI;
}
