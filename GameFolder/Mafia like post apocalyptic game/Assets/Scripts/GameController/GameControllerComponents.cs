using UnityEngine;

public class GameControllerComponents : MonoBehaviour
{
    public static GameControllerComponents instance;

    [Header("UI COMPONENTS")]
    [SerializeField] GlobalInputs globalInputs;
    [SerializeField] GameUI gameUI;
    [SerializeField] GameUITexts gameUiText;
    [SerializeField] TextFX textFX;
    [SerializeField] VoteTab voteTab;
    [SerializeField] DeathTab deathTab;
    [SerializeField] AbilityButtonsTabController abilityButtonsTabController;
    [SerializeField] OnlinePlayersList onlinePlayersList;

    [Header("AUDIO")]
    [SerializeField] UISoundsInGame uISoundsInGame;

    [Header("COMPONENTS")]
    [SerializeField] InstantiatePlayers instantiatePlayers;
    [SerializeField] GameStart gameStart;
    [SerializeField] SyncPlayersTimer syncPlayersTimer;
    [SerializeField] VFXHolder vfxHolder;
    [SerializeField] GameControllerRPC gameControllerRPC;
    [SerializeField] ObjectsHolder objectsHolder;
    [SerializeField] NetworkCallbacks networkCallbacks;

    public GlobalInputs GlobalInputs => globalInputs;
    public GameUI GameUI => gameUI;
    public GameUITexts GameUITexts => gameUiText;
    public InstantiatePlayers InstantiatePlayers => instantiatePlayers;
    public GameStart GameStart => gameStart;
    public SyncPlayersTimer SyncPlayersTimer => syncPlayersTimer;
    public VFXHolder VFXHolder => vfxHolder;
    public TextFX TextFX => textFX;
    public GameControllerRPC GameControllerRPC => gameControllerRPC;
    public ObjectsHolder ObjectsHolder => objectsHolder;
    public VoteTab VoteTab => voteTab;
    public DeathTab DeathTab => deathTab;
    public UISoundsInGame UISoundsInGame => uISoundsInGame;
    public AbilityButtonsTabController AbilityButtonsTabController => abilityButtonsTabController;
    public NetworkCallbacks NetworkCallbacks => networkCallbacks;
    public OnlinePlayersList OnlinePlayersList => onlinePlayersList;


    void Awake()
    {
        instance = this;
    }

}
