using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    [SerializeField] float second;
    void Update()
    {
        Destroy(gameObject, second);
    }
}
