using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider loadingBar;

    void Start()
    {
        StartCoroutine(RunUntilFirstTabOpened());
    }

    #region RunUntilFirstTabOpened coroutine
    IEnumerator RunUntilFirstTabOpened()
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneNames.MenuScene);
        operation.allowSceneActivation = false;

        while (operation.progress <= 0.9f && loadingBar.value < Random.Range(0.4f, 0.75f)) 
        {
            loadingBar.value += operation.progress * Time.deltaTime;  
            yield return null;
        }

        do
        {
            loadingBar.value += 5 * Time.deltaTime;
            yield return null;
        }
        while (operation.progress >= 0.9f && loadingBar.value < 1);

        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }
    #endregion










}
