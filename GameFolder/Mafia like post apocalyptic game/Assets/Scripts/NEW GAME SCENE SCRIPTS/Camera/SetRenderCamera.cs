using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderCamera : MonoBehaviour
{
    Options _Options;

    void Awake()
    {
        _Options = FindObjectOfType<Options>();
    }

    void Start()
    {
        _Options.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
