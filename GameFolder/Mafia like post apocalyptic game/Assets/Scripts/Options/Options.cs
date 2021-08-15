using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviourPun
{
    public static Options instance;

    public OptionsUI _OptionsUI;
  
    #region OptionsUI
    [Serializable]
    public class OptionsUI
    {
        [Header("CANVAS GROUP")]
        [SerializeField] CanvasGroup optionsTab;
        [SerializeField] CanvasGroup optionsButtonTab;

        public CanvasGroup OptionsTab => optionsTab;
        public CanvasGroup OptionsButtonTab => optionsButtonTab;
    }
    #endregion

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
     
    public void OnPressedOptionButton()
    {
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, true);
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, false);        
    }

    public void OnPressedOptionsButtons()
    {
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsTab, false);
        MyCanvasGroups.CanvasGroupActivity(_OptionsUI.OptionsButtonTab, true);        
    }
}
