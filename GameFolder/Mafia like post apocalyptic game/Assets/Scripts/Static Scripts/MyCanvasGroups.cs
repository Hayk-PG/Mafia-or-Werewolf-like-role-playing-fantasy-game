using UnityEngine;

public class MyCanvasGroups : MonoBehaviour
{
    public static void CanvasGroupActivity(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
}
