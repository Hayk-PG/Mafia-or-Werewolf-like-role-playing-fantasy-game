using System;
using UnityEngine;
using UnityEngine.UI;

public class CardsHolder : MonoBehaviour
{
    [Serializable] public class Transforms
    {
        [SerializeField] RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get => rectTransform;
        }
    }
    [Serializable] public class Conditions
    {
        [SerializeField] bool maximize;
        [SerializeField] bool isMaximized;

        public bool Maximize
        {
            get => maximize;
            set => maximize = value;
        }
        public bool IsMaximized
        {
            get => isMaximized;
            set => isMaximized = value;
        }
    }
    [Serializable] public class Other
    {
        [SerializeField] GameObject backgroundObj;

        public GameObject BackgroundObj
        {
            get => backgroundObj;
        }
    }

    public Transforms _Transforms;
    public Conditions _Conditions;
    public Other _Other;

    void Awake()
    {
        _Transforms.RectTransform.offsetMin = new Vector2(0, 0);
        _Transforms.RectTransform.offsetMax = new Vector2(0, -230);
        _Transforms.RectTransform.anchorMax = new Vector2(0.9710001f, 0);
    }

    void Update()
    {
        if (_Conditions.Maximize)
        {
            if (!_Conditions.IsMaximized)
            {
                Maximize();
            }
            else
            {
                Minimize();
            }
        }
    }

    void Maximize()
    {
        if (_Transforms.RectTransform.anchorMax.y != 0.234f) _Transforms.RectTransform.anchorMax = new Vector2(0.9710001f, 0.234f);

        _Transforms.RectTransform.offsetMax = Vector2.Lerp(_Transforms.RectTransform.offsetMax, new Vector2(0, 0), 25 * Time.deltaTime);

        if (_Transforms.RectTransform.offsetMax.y >= -0.1f && _Transforms.RectTransform.offsetMax.y <= 0.1f)
        {
            _Transforms.RectTransform.offsetMax = Vector2.zero;
            _Conditions.IsMaximized = true;
            _Conditions.Maximize = false;
        }
    }

    void Minimize()
    {
        _Transforms.RectTransform.offsetMax = Vector2.Lerp(_Transforms.RectTransform.offsetMax, new Vector2(0, -230), 25 * Time.deltaTime);

        if (_Transforms.RectTransform.offsetMax.y >= -231 && _Transforms.RectTransform.offsetMax.y <= -229)
        {
            _Transforms.RectTransform.offsetMax = new Vector2(0, -230);
            _Transforms.RectTransform.anchorMax = new Vector2(0.9710001f, 0);
            _Conditions.IsMaximized = false;
            _Conditions.Maximize = false;
        }
    }

    public void CardsInteractable(bool isInteractable)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = isInteractable;
        }
    }

    public void CardIsClicked(bool isActive)
    {
        _Other.BackgroundObj.SetActive(isActive);
    }
}
