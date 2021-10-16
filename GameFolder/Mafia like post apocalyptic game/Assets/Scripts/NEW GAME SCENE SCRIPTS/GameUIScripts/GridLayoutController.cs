using UnityEngine;
using UnityEngine.UI;

public class GridLayoutController : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] Transform gridLayoutTransform;

    TextAnchor ChildAlignment
    {
        get => gridLayoutGroup.childAlignment;
        set => gridLayoutGroup.childAlignment = value;
    }

    void Update()
    {
        if(gridLayoutTransform.childCount == 1)
            ChildAlignment = TextAnchor.UpperCenter;
        else if(gridLayoutTransform.childCount == 2)
            ChildAlignment = TextAnchor.MiddleCenter;
        else
            ChildAlignment = TextAnchor.UpperLeft;
    }
}
