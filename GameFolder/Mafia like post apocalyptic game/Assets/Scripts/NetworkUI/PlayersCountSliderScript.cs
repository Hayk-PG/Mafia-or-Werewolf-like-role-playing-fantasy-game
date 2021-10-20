using UnityEngine;
using UnityEngine.UI;

public class PlayersCountSliderScript : MonoBehaviour
{
    [SerializeField] Text playersCountText;

    string PlayersCountString
    {
        get
        {
            return playersCountText.text;
        }
        set
        {
            playersCountText.text = value;
        }
    }

    void Update()
    {
        PlayersCountString = GetComponent<Slider>().value.ToString();
    }

    public void OnValueChanged()
    {
        PlayerBaseConditions.UiSounds.PlaySoundFX(2);
    }



}
