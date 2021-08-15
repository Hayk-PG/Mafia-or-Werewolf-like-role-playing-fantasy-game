using UnityEngine;
using UnityEngine.UI;

public class GameUITexts : MonoBehaviour
{
    [Header("TIMER TEXT")]
    [SerializeField] Text timerText;

    [Header("TEAMS TEXT")]
    [SerializeField] Text humansCountText;
    [SerializeField] Text infectedsCountText;

    [Header("ANIMATOR")]
    [SerializeField] Animator[] anims;

    /// <summary>
    /// Timer
    /// </summary>
    public Text TimerText => timerText;

    /// <summary>
    /// Humans count 
    /// </summary>
    public int HumansCount
    {
        get
        {
            return int.Parse(humansCountText.text);
        }
        set
        {
            humansCountText.text = value.ToString();
        }
    }

    /// <summary>
    /// Infecteds count
    /// </summary>
    public int InfectedsCount
    {
        get
        {
            return int.Parse(infectedsCountText.text);
        }
        set
        {
            infectedsCountText.text = value.ToString();
        }
    }

    /// <summary>
    /// 0: Humans text anim 1: Infecteds text anim
    /// </summary>
    public Animator[] Anims => anims;














}
