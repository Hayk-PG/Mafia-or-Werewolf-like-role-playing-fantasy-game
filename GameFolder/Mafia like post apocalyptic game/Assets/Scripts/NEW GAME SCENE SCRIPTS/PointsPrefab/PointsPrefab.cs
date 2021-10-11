﻿using UnityEngine;
using UnityEngine.UI;

public class PointsPrefab : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] int destroyTime;

    public string Text
    {
        get => text.text;
        set => text.text = value;
    }

    void Start()
    {
        Invoke("Destroy", destroyTime);
    }

    void Destroy()
    {
        GameObject explosionVFXCopy = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        PlayerBaseConditions.UiSounds.PlaySoundFXinGame(7);
        Destroy(gameObject);
    }
}
