
public class OnClickSinglePlayerButton : OptionButtonsBaseScript
{
    protected override void Logic()
    {
        _MySceneManager.LoadScene(SceneNames.SinglePlayerScene);
        Options.instance.OnPressedOptionsButtons();
    }
}
