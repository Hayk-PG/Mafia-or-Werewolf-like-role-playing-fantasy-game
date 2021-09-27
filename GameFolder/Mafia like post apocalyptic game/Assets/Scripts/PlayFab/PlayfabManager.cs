using UnityEngine;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager PM;

    public PlayfabRegister PlayfabSignUp { get; set; }
    public PlayfabLogin PlayfabSignIn { get; set; }
    public PlayfabUserData PlayfabUserData { get; set; }
    public PlayfabUserProfile PlayfabUserProfile { get; set; }
    public PlayfabIsLoggedIn PlayfabIsLoggedIn { get; set; }
    public PlayfabLogOut PlayfabLogOut { get; set; }
    public PlayfabStats PlayfabStats { get; set; }
    public PlayfabFriends PlayfabFriends { get; set; }
    public PlayfabEntity PlayfabEntity { get; set; }
    public PlayfabFile PlayfabFile { get; set; }
    public PlayfabUserAccountInfo PlayfabUserAccountInfo { get; set; }
    public PlayfabInternalData PlayfabInternalData { get; set; }
    public PlayfabDeleteAccount PlayfabDeleteAccount { get; set; }


    void Awake()
    {
        if(PM == null)
        {
            PM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayfabSignUp = GetComponent<PlayfabRegister>();
        PlayfabSignIn = GetComponent<PlayfabLogin>();
        PlayfabUserData = GetComponent<PlayfabUserData>();
        PlayfabUserProfile = GetComponent<PlayfabUserProfile>();
        PlayfabIsLoggedIn = GetComponent<PlayfabIsLoggedIn>();
        PlayfabLogOut = GetComponent<PlayfabLogOut>();
        PlayfabStats = GetComponent<PlayfabStats>();
        PlayfabFriends = GetComponent<PlayfabFriends>();
        PlayfabEntity = GetComponent<PlayfabEntity>();
        PlayfabFile = GetComponent<PlayfabFile>();
        PlayfabUserAccountInfo = GetComponent<PlayfabUserAccountInfo>();
        PlayfabInternalData = GetComponent<PlayfabInternalData>();
        PlayfabDeleteAccount = GetComponent<PlayfabDeleteAccount>();
    }









}
