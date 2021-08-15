using UnityEngine;
using UnityEngine.UI;

public class FlagsTab : MonoBehaviour
{
    public static string CountryCode;

    [Header("PREFABS")]
    [SerializeField] Button flagPrefab;

    [Header("TRANSFORM")]
    [SerializeField] Transform flagsContainer;

    [Header("BUTTON")]
    [SerializeField] CanvasGroup confirmButtonCanvasGroup;


    void Awake()
    {
        OnFlagButtonsInitialization();
    }

    void Update()
    {
        OnClickFlagsButtons();
    }

    #region OnFlagButtonsInitialization
    void OnFlagButtonsInitialization()
    {
        for (int i = 0; i < Flags.instance.FlagSprites.Length; i++)
        {
            Button flagCopy = Instantiate(flagPrefab, flagsContainer);
            flagCopy.image.sprite = Flags.instance.FlagSprites[i];

            int startIndex = 5;
            int length = Flags.instance.FlagSprites[i].name.Length - startIndex;

            flagCopy.name = Flags.instance.FlagSprites[i].name.Substring(startIndex, length);
        }
    }
    #endregion

    #region CapScrollYAxis
    void CapScrollYAxis(ScrollRect scroll)
    {
        if (scroll.content.anchoredPosition.y < 0)
        {
            scroll.content.anchoredPosition = new Vector2(0, 0);
        }
        if (scroll.content.anchoredPosition.y > 595)
        {
            scroll.content.anchoredPosition = new Vector2(0, 595);
        }
    }
    #endregion

    #region OnScrollValueChanged
    public void OnScrollValueChanged(ScrollRect scroll)
    {
        CapScrollYAxis(scroll);
    }
    #endregion
    
    #region OnClickFlagsButtons
    void OnClickFlagsButtons()
    {
        for (int i = 0; i < flagsContainer.childCount; i++)
        {
            int buttonIndex = i;

            flagsContainer.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            flagsContainer.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
            {
                GetCountryCode(flagsContainer.GetChild(buttonIndex).name);
                SelectFlag(flagsContainer.GetChild(buttonIndex).GetComponent<Button>());
                EnableConfirmButton();
            });
        }
    }
    #endregion

    #region GetCountryCode
    void GetCountryCode(string buttonName)
    {
        CountryCode = buttonName;
    }
    #endregion

    #region SelectFlag
    void SelectFlag(Button flagButton)
    {
        for (int i = 0; i < flagsContainer.childCount; i++)
        {
            MyCanvasGroups.CanvasGroupActivity(flagsContainer.GetChild(i).Find("Selected").GetComponent<CanvasGroup>(), false);
        }

        MyCanvasGroups.CanvasGroupActivity(flagButton.transform.Find("Selected").GetComponent<CanvasGroup>(), true);
    }
    #endregion

    #region EnableConfirmButton
    void EnableConfirmButton()
    {
        if (!confirmButtonCanvasGroup.interactable)
        {
            MyCanvasGroups.CanvasGroupActivity(confirmButtonCanvasGroup, true);
        }
    }
    #endregion










}
