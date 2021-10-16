using System;
using UnityEngine;
using UnityEngine.UI;

public class EndTab : MonoBehaviour
{
    [Serializable] public struct UI
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Text titleText;

        public CanvasGroup CanvasGroup
        {
            get => canvasGroup;
        }
        public string Title
        {
            get => titleText.text;
            set => titleText.text = value;
        }
    }
    [Serializable] class AwarededPlayerCard
    {
        [SerializeField] internal AwardedPlayerCardController awardedPlayerCardController;
        [SerializeField] internal Transform GridLayoutTransform;
    }
    public UI _UI;
    [SerializeField] AwarededPlayerCard _AwarededPlayerCard;


    public void OpenEndTab()
    {
        MyCanvasGroups.CanvasGroupActivity(_UI.CanvasGroup, true);
    }

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
}
