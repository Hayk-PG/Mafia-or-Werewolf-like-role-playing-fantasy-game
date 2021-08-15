using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [Header("DESTROY TIME")]
    [SerializeField] float destroyTime;


    void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    


}
