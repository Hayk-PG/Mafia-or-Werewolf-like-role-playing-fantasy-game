using UnityEngine;

public class Flags : MonoBehaviour
{
    public static Flags instance;

    [Header("SPRITES")]
    [SerializeField] Sprite[] flags;

    public Sprite[] FlagSprites => flags;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
