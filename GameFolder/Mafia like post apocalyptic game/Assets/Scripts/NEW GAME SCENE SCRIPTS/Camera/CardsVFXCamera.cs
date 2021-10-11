
public class CardsVFXCamera : CameraBaseActivity
{
    void Update()
    {
        if (_Options._OptionsUI.OptionsTab.interactable) enabled = false;
        else enabled = true;
    }
}
