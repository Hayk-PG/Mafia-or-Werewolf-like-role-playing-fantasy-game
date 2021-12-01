
public class OnClickMainMenu : OptionButtonsBaseScript
{
    protected override void Logic()
    {
        if (_MySceneManager.CurrentScene().name != SceneNames.MenuScene)
        {
            _MySceneManager.ChangeToMenuScene();
            Options.instance.OnPressedOptionsButtons();
        }
    }
}
