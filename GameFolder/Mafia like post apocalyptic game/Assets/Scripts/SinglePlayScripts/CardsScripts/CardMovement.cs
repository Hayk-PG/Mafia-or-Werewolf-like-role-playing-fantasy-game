using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour
{
    [Serializable] public class UI
    {
        [SerializeField] Image cardMainImage;
        [SerializeField] Image cardBorderImage;
        [SerializeField] Image iconImage;
        [SerializeField] Button cardButton;

        public GameObject CardBorderImageObj
        {
            get => cardBorderImage.gameObject;
        }
        public GameObject IconImageObj
        {
            get => iconImage.gameObject;
        }
        public Sprite CardMainImage
        {
            get => cardMainImage.sprite;
            set => cardMainImage.sprite = value;
        }
        public Sprite Icon
        {
            get => iconImage.sprite;
            set => iconImage.sprite = value;
        }
        public Button CardButton
        {
            get => cardButton;
        }
    }
    [Serializable] public class Prefabs
    {
        [SerializeField] Sprite cardFrontImagePrefab;
        [SerializeField] Sprite[] iconsPrefabs;

        public Sprite CardFrontImagePrefab
        {
            get => cardFrontImagePrefab;
        }
        public Sprite[] IconsPrefabs
        {
            get => iconsPrefabs;
        }
    }
    [Serializable] public class Transforms
    {
        [SerializeField] RectTransform rectTransform;
        [SerializeField] float distance;

        public RectTransform RectTransform
        {
            get => rectTransform;
        }
        public Transform GameUiTransform
        {
            get => GameObject.Find("GameUI").transform;
        }
        public Transform CardsHolderTransform { get; set; }
        public float Distance
        {
            get => distance;
            set => distance = value;
        }
    }
    [Serializable] public class Conditions
    {
        [SerializeField] bool isMoving;
        [SerializeField] bool isScaled;

        public bool IsMoving
        {
            get => isMoving;
            set => isMoving = value;
        }
        public bool IsScaled
        {
            get => isScaled;
            set => isScaled = value;
        }
    }
    [Serializable] public class Other
    {
        [SerializeField] Animator anim;
        [SerializeField] ParticleSystem[] vfx;

        public Animator Anim
        {
            get => anim;
        }
        public ParticleSystem[] Vfx
        {
            get => vfx;
        }
    }

    public UI _UI;
    public Prefabs _Prefabs;
    public Transforms _Transforms;
    public Conditions _Conditions;
    public Other _Other;
    CardsHolder _CardsHolder;


    void Awake()
    {
        _Transforms.CardsHolderTransform = transform.parent;
        _CardsHolder = FindObjectOfType<CardsHolder>();
    }

    void Update()
    {
        _UI.CardButton.onClick.RemoveAllListeners();
        _UI.CardButton.onClick.AddListener(() => 
        {
            _Conditions.IsMoving = true;
            _CardsHolder._Conditions.Maximize = true;

            for (int i = 0; i < _Transforms.CardsHolderTransform.childCount; i++)
            {
                _Transforms.CardsHolderTransform.GetChild(i).GetComponent<CardMovement>()._UI.CardButton.interactable = false;
            }
        });

        if (_Conditions.IsMoving)
        {
            if (transform.parent != _Transforms.GameUiTransform)
            {
                transform.SetParent(_Transforms.GameUiTransform);
                transform.SetAsLastSibling();
            }
            
            transform.position = Vector2.Lerp(transform.position, Vector2.zero, 10 * Time.deltaTime);
            _Transforms.RectTransform.sizeDelta = Vector2.Lerp(_Transforms.RectTransform.sizeDelta, new Vector2(317.3334f, 462.6666f), 10 * Time.deltaTime);
            _Transforms.Distance = Vector2.Distance(transform.position, Vector2.zero);

            if (_Transforms.Distance <= 1 && _Conditions.IsScaled == false)
            {
                _Other.Anim.SetTrigger("play");
                foreach (var item in _Other.Vfx)
                {
                    item.Play();
                }
                _Conditions.IsScaled = true;
            }

            if (transform.position == (Vector3)Vector2.zero)
            {
                _Conditions.IsMoving = false;
            }
        }
    }

    public void CardOpen()
    {
        _UI.CardMainImage = _Prefabs.CardFrontImagePrefab;
        _UI.CardBorderImageObj.SetActive(true);
        _UI.IconImageObj.SetActive(true);
        _UI.Icon = _Prefabs.IconsPrefabs[0];
    }
}
