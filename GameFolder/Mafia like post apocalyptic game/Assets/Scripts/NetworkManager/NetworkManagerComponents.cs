using UnityEngine;

public class NetworkManagerComponents: MonoBehaviour
{
    public static NetworkManagerComponents Instance;
       
    [Header("COMPONENTS")]
    [SerializeField] NetworkManager networkManager;
    [SerializeField] NetworkManagerUI networkUI;
    [SerializeField] NetworkManagerUIButtons networkUiButtons;
    [SerializeField] NetworkObjectsHolder networkObjectsHolder;
    [SerializeField] NetworkManagerCreatedRoomProperties networkManagerCreatedRoomProperties;

    [Header("TABS")]
    [SerializeField] SignUpTab signUpTab;
    [SerializeField] SignInTab signInTab;

    public NetworkManager NetworkManager => networkManager;
    public NetworkManagerUI NetworkUI => networkUI;
    public NetworkManagerUIButtons NetworkUIButtons => networkUiButtons;
    public NetworkObjectsHolder NetworkObjectsHolder => networkObjectsHolder;
    public NetworkManagerCreatedRoomProperties NetworkManagerCreatedRoomProperties => networkManagerCreatedRoomProperties;
    public SignUpTab SignUpTab => signUpTab;
    public SignInTab SignInTab => signInTab;


    void Awake()
    {
        Instance = this;
    }








}
