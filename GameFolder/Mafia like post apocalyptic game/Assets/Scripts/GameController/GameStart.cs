using Photon.Pun;
using UnityEngine;

public class GameStart : MonoBehaviourPun, IStartTheGame
{
    [Header("GAME START")]
    [SerializeField] bool isGameStarted;
    public bool IsGameStarted
    {
        get
        {
            return isGameStarted;
        }
        set
        {
            isGameStarted = value;
        }
    }


    void Awake()
    {
        Application.targetFrameRate = 30;
    }

    void OnEnable()
    {
        GetComponent<GameControllerComponents>().GlobalInputs.OnClickGameStartButton += GlobalInputs_OnClickGameStartButton;
    }   

    void OnDisable()
    {
        GetComponent<GameControllerComponents>().GlobalInputs.OnClickGameStartButton -= GlobalInputs_OnClickGameStartButton;
    }

    /// <summary>
    /// 1: Closes the room 2: Hides GameStartButton
    /// </summary>
    /// <param name="actorNumber"></param>
    void GlobalInputs_OnClickGameStartButton(int actorNumber)
    {        
        photonView.RPC("OnClickGameStartButtonRPC", RpcTarget.All);
    }

    /// <summary>
    /// 1: Hides GameStartTab 2: IsGameStarted = true
    /// </summary>
    public void StartTheGame()
    {
        photonView.RPC("OnStartTheGame", RpcTarget.All);
    }


















}
