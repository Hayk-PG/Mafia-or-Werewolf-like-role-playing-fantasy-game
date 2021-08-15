using UnityEngine;
using UnityEngine.UI;

public class GetTimer : MonoBehaviour
{
    public Text TimerText { get; set; }
    public string Timer
    {
        get
        {
            if(TimerText != null)
            {
                return TimerText.text;
            }
            else
            {
                return null;
            }
        }
        set
        {
            TimerText.text = value;
        }
    }

   
    void Awake()
    {
        TimerText = FindObjectOfType<GameControllerComponents>().GameUITexts.TimerText;
    }










}
