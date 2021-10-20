using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndTab : MonoBehaviour,IReset
{
    [Serializable] public struct UI
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] CanvasGroup scoresTabCanvasGroup;
        [SerializeField] Image background;
        [SerializeField] Text titleText;

        [SerializeField] Text publicScoresText;
        [SerializeField] Text yourScoresText;

        [SerializeField] internal Color[] textColors;
 
        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
        }
        public CanvasGroup ScoresTabCanvasGroup
        {
            get => scoresTabCanvasGroup;
        }
        public string Title
        {
            get => titleText.text;
            set => titleText.text = value;
        }
        public Color32 BackgroundColor
        {
            get => background.color;
            set => background.color = value;
        }
        public string PublicScores
        {
            get => publicScoresText.text;
            set => publicScoresText.text = value;
        }
        public string YourScores
        {
            get => yourScoresText.text;
            set => yourScoresText.text = value;
        }
    }
    [Serializable] class AwarededPlayerCard
    {
        [SerializeField] internal AwardedPlayerCardController awardedPlayerCardController;
        [SerializeField] internal Transform GridLayoutTransform;
    }
    public UI _UI;
    [SerializeField] AwarededPlayerCard _AwarededPlayerCard;

    GameManagerTimer _GameManagerTimer { get; set; }


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
    }

    #region OpenEndTab
    public void OpenEndTab()
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);
        StartCoroutine(BackgroundAnimation());
    }
    #endregion

    #region DisplayPublicScores
    public void DisplayPublicScores(string roleName, string playerName, string score)
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.ScoresTabCanvasGroup, true);
        _UI.PublicScores += "<color=#ffa500ff> (" + roleName + ")</color>" + " " + playerName + "   +" + "<color=#00ff00ff>" + score + "</color>" + "\n";
    }
    #endregion

    #region DisplayYourScores
    public void DisplayYourScores(string roleName, string playerName, string score)
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.ScoresTabCanvasGroup, true);
        _UI.YourScores += "<color=#ffa500ff> (" + roleName + ")</color>" + " " + playerName + "  <color=#00ff00ff> +" + score + "</color>" + "\n";
    }
    #endregion

    #region InstantiateAwarededPlayerCard

    /// <summary>
    /// 0: One star 1: Two stars 2: Three star 3: Best infected 4: Best lizard 5: Best medic 6: Best medic 7: Best sheriff 8: Best soldier 9: Highest vote
    /// </summary>
    public void InstantiateAwarededPlayerCard(Sprite picture, string title, string award, int starsIndex, bool isLocalCard)
    {
        AwardedPlayerCardController awardedPlayerCardControllerCopy = Instantiate(_AwarededPlayerCard.awardedPlayerCardController, _AwarededPlayerCard.GridLayoutTransform);
        awardedPlayerCardControllerCopy._UI.Picture = picture;
        awardedPlayerCardControllerCopy._UI.Title = title;
        awardedPlayerCardControllerCopy._UI.Award = award;
        awardedPlayerCardControllerCopy._UI.Stars = awardedPlayerCardControllerCopy._UI.starsSprites[starsIndex];

        if (isLocalCard)
            awardedPlayerCardControllerCopy.transform.SetAsLastSibling();
    }
    #endregion

    #region BackgroundAnimation
    IEnumerator BackgroundAnimation()
    {
        float rgb = _UI.BackgroundColor.r;

        if(_UI.BackgroundColor.a != 255)
        {
            _UI.BackgroundColor = new Color32(0, 0, 0, 255);
        }

        yield return new WaitForSeconds(1);

        while (_UI.BackgroundColor.a >= 255 && _UI.BackgroundColor.r < 255)
        {
            rgb += 255 * Time.deltaTime;
            _UI.BackgroundColor = new Color32((byte)Mathf.Clamp(rgb, 0, 255), (byte)Mathf.Clamp(rgb, 0, 255), (byte)Mathf.Clamp(rgb, 0, 255), 255);
            yield return null;
        }

        _UI.BackgroundColor = Color.white;
    }
    #endregion

    #region IReset  
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, false);
        MyCanvasGroups.CanvasGroupActivity(_UI.ScoresTabCanvasGroup, false);
        _UI.BackgroundColor = new Color32(0, 0, 0, 0);
        _UI.PublicScores = "";
        _UI.YourScores = "";
    }
    #endregion
}
