using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryDatas : MonoBehaviour
{
    [Header("AVATAR REAL NAMES")]
    [SerializeField] List<string> avatarRealNames = new List<string>();

    public List<string> AvatarRealNames
    {
        get
        {
            return avatarRealNames;
        }
        set
        {
            avatarRealNames = value;
        }
    }
}
