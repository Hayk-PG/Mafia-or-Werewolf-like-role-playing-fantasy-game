using UnityEngine;
using UnityEngine.UI;

public class TabButtons : MonoBehaviour
{
    [SerializeField] Image[] images;


    public void ImagesColor(Color32 color)
    {
        foreach (var image in images)
        {
            image.color = color;
        }
    }
}
