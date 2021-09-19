using UnityEngine;
using UnityEngine.UI;

public class DeathCardRotationImageChange : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Sprite frontImage, backImage;

    Sprite MainImage
    {
        get => mainImage.sprite;
        set => mainImage.sprite = value;
    }


    public void FrontImage()
    {
        MainImage = frontImage;
    }

    public void BackImage()
    {
        MainImage = backImage;
    }
}
