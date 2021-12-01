using UnityEngine;

public class SetRenderCamera : MonoBehaviour
{
    Camera mainCamera;
    Options _Options;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        _Options = FindObjectOfType<Options>();
    }

    void Start()
    {
        _Options.GetComponent<Canvas>().worldCamera = mainCamera;
    }
}
