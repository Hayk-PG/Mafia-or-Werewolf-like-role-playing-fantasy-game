using UnityEngine;

public class VFXActivity : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    void OnEnable()
    {
        particles.Play();
    }
}
