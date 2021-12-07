using UnityEngine;

public class RectTransformAnchorController : MonoBehaviour
{
    RectTransform thisRT;
    RectTransform parentsRT;

    void Start()
    {
        thisRT = gameObject.GetComponent<RectTransform>();
        parentsRT = gameObject.GetComponent<RectTransform>().parent as RectTransform;

        if (thisRT == null || parentsRT == null) return;

        Vector2 newAnchorsMin = new Vector2(thisRT.anchorMin.x + thisRT.offsetMin.x / parentsRT.rect.width, thisRT.anchorMin.y + thisRT.offsetMin.y / parentsRT.rect.height);
        Vector2 newAnchorsMax = new Vector2(thisRT.anchorMax.x + thisRT.offsetMax.x / parentsRT.rect.width, thisRT.anchorMax.y + thisRT.offsetMax.y / parentsRT.rect.height);

        thisRT.anchorMin = newAnchorsMin;
        thisRT.anchorMax = newAnchorsMax;
        thisRT.offsetMin = new Vector2(0, 0);
        thisRT.offsetMax = new Vector2(0, 0);
    }
}
