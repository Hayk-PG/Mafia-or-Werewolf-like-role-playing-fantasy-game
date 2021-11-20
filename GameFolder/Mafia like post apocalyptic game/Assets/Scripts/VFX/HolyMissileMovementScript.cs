using UnityEngine;

public class HolyMissileMovementScript : MonoBehaviour
{
    [SerializeField] GameObject explosionVFX;

    SinglePlayerRoleButtonsContainersController _SPRBCC;

    float x;
    float distance;


    void Awake()
    {
        _SPRBCC = FindObjectOfType<SinglePlayerRoleButtonsContainersController>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, _SPRBCC._Transforms.HolyMissileVfxDest.position);       
        transform.position = Vector2.Lerp(transform.position, _SPRBCC._Transforms.HolyMissileVfxDest.position, 5 * Time.deltaTime);
        DestroyVfx(distance);
    }

    void DestroyVfx(float distance)
    {
        if(distance <= 0.1f)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject expl = Instantiate(explosionVFX, pos, Quaternion.identity, transform.parent);
            _SPRBCC._Animators.SwitchLostPlayersScreenButtonAnim.SetTrigger("play");
            Destroy(gameObject);
        }
    }
}
