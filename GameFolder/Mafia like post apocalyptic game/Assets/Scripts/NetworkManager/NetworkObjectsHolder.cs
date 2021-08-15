using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectsHolder : MonoBehaviour
{
    [Header("PREFABS")]
    [SerializeField] RoomButtonScript buttonPrefab;

    [Header("TRANSFORM")]
    public Transform roomsContainer;

    public RoomButtonScript RoomButtonPrefab => buttonPrefab;

}
