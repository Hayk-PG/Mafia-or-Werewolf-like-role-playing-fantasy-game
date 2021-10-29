using UnityEngine;

public class AbilitySoundFX : MonoBehaviour
{
    [SerializeField] int soundIndex;

    void Awake()
    {
        PlayerBaseConditions.UiSounds.PlayAblitySoundFX(soundIndex);
    }
}
