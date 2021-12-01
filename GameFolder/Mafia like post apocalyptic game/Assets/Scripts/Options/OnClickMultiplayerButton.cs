
public class OnClickMultiplayerButton : OptionButtonsBaseScript
{   
    protected override void Logic()
    {
        if (PlayerBaseConditions.IsConnectedToMaster() && PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PlayerBaseConditions.JoinLobby();
        }
        else
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkManager.connectionType = NetworkManager.ConnectionType.Auto;
            PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedOut();
        }
    }
}
