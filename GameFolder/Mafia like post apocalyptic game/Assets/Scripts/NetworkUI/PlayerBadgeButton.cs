using UnityEngine;

public class PlayerBadgeButton : MonoBehaviour
{
    [SerializeField] Transform vfxSpawnPoint;
    [SerializeField] GameObject vfxPrefab;

    public void OnPlayerLoggedIn()
    {
        GameObject vfxPrefabCopy = Instantiate(vfxPrefab, vfxSpawnPoint.position, Quaternion.identity);
    }
}
