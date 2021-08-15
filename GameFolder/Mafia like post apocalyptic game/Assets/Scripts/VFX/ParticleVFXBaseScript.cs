using UnityEngine;

public class ParticleVFXBaseScript : MonoBehaviour, IParticleVFXBaseScript
{
    [Header("VFX")]
    [SerializeField] GameObject destroyVFX;
    public GameObject DestroyVFX => destroyVFX;



    public void ManualDestroy()
    {
        GameObject destroyVFXCopy = Instantiate(DestroyVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }





}
