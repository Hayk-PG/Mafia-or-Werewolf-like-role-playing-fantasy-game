using UnityEngine;

public class ObjectsHolder : MonoBehaviour
{
    [Header("TRANSFORMS")]
    [SerializeField] Transform timerTickerSpawnPoint;

    public Transform TimerTickerSpawnPoint => timerTickerSpawnPoint;



}
