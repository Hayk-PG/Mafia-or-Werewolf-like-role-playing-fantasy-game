
public class OnClickExit : OptionButtonsBaseScript
{
    protected override void Logic()
    {
        Options.instance.OnPressedExitButton(true);
    }
}
