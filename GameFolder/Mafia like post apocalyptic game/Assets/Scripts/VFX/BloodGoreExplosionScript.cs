using UnityEngine;

public class BloodGoreExplosionScript : MonoBehaviour
{
    [Header("AUDIO CLIP")]
    [SerializeField] AudioClip bloodGoreExplosionSoundFX;

    void OnEnable()
    {
        //PlayerBaseConditions._MyGameControllerComponents.UISoundsInGame.PlayUISoundFX(bloodGoreExplosionSoundFX);
    }
}
