using System;
using UnityEngine;
using UnityEngine.UI;

public class UploadImage : MonoBehaviour
{
    [Serializable] public struct UI
    {
        [SerializeField] Button backButton;
        [SerializeField] Button uploadButton;
        [SerializeField] Button signUpButton;

        [SerializeField] Image profilePic;

        [SerializeField] CanvasGroup uploadImageCanvasGroup;
        [SerializeField] CanvasGroup signUpCanvasGroup;

        [SerializeField] internal Sprite sprite;

        internal Button BackButton
        {
            get => backButton;
        }
        internal Button UploadButton
        {
            get => uploadButton;
        }
        internal Button SignUpButton
        {
            get => signUpButton;
        }
        internal Sprite ProfilePic
        {
            get => profilePic.sprite;
            set => profilePic.sprite = value;
        }
        internal CanvasGroup UploadImageCanvasGroup
        {
            get => uploadImageCanvasGroup;
        }
        internal CanvasGroup SignUpCanvasGroup
        {
            get => signUpCanvasGroup;
        }
    }
    class RegInfo
    {
        internal string Username { get; set; }
        internal string Password { get; set; }
        internal SignUpTab.Gender Gender { get; set; }
        internal bool HasSaved { get; set; }

        public RegInfo()
        {

        }

        public RegInfo(string Username, string Password, SignUpTab.Gender Gender, bool HasSaved)
        {
            this.Username = Username;
            this.Password = Password;
            this.Gender = Gender;
            this.HasSaved = HasSaved;
        }
    }

    public UI _UI;

    SignUpTab _SignUpTab;
    RegInfo _RegInfo;
    AndroidGoodiesExamples.OtherGoodiesTest Android;


    void Awake()
    {
        _SignUpTab = FindObjectOfType<SignUpTab>();
        Android = FindObjectOfType<AndroidGoodiesExamples.OtherGoodiesTest>();
    }

    void OnEnable()
    {
        _SignUpTab.OnClickContinueButton += _SignUpTab_OnClickContinueButton;
    }

    void OnDisable()
    {
        _SignUpTab.OnClickContinueButton += _SignUpTab_OnClickContinueButton;
    }

    void Update()
    {
        OnClickButton(_UI.BackButton);
        OnClickButton(_UI.UploadButton);
        OnClickButton(_UI.SignUpButton);

        _UI.SignUpButton.interactable = _UI.ProfilePic.texture != null && _UI.ProfilePic.name != "GalleryIcon" ? true : false;
    }

    void _SignUpTab_OnClickContinueButton(string arg1, string arg2, SignUpTab.Gender arg3, bool arg4)
    {
        _RegInfo = new RegInfo {Username = arg1, Password = arg2, Gender = arg3, HasSaved = arg4 };
        MyCanvasGroups.CanvasGroupActivity(_UI.SignUpCanvasGroup, false);
        MyCanvasGroups.CanvasGroupActivity(_UI.UploadImageCanvasGroup, true);
    }

    void OnClickButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (button == _UI.BackButton)
            {
                MyCanvasGroups.CanvasGroupActivity(_UI.SignUpCanvasGroup, true);
                MyCanvasGroups.CanvasGroupActivity(_UI.UploadImageCanvasGroup, false);
            }
            if(button == _UI.UploadButton)
            {
                Android.OnPickGalleryImage();
                //_UI.ProfilePic = _UI.sprite;
            }
            if(button == _UI.SignUpButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabSignUp.OnPlayfabRegister(_RegInfo.Username, _RegInfo.Password, _RegInfo.Gender);

                if (_RegInfo.HasSaved) PlayerBaseConditions.PlayerSavedData.SaveUsernameAndPassword(_RegInfo.Username, _RegInfo.Password);
                else PlayerBaseConditions.PlayerSavedData.DeleteUsernameAndPassword();
            }
        });  
    }


    
}
