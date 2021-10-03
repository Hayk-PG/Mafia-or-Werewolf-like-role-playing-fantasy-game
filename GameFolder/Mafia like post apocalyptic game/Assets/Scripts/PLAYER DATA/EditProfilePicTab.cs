using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EditProfilePicTab : MonoBehaviour
{
    [Serializable] struct UI
    {
        [SerializeField] internal Button uploadImageButton;
        [SerializeField] internal Button confirmButton;
        [SerializeField] internal Button backButton;
    }
    [SerializeField] UI _UI;

    Profile _Profile;
    AndroidGoodiesExamples.OtherGoodiesTest _Android;

    void Awake()
    {
        _Profile = FindObjectOfType<Profile>();
        _Android = FindObjectOfType<AndroidGoodiesExamples.OtherGoodiesTest>();

        OnClickButton(_UI.uploadImageButton);
        OnClickButton(_UI.confirmButton);
        OnClickButton(_UI.backButton);
    }

    void OnEnable()
    {
        _Android.OnPickProfileImage += OnPickImageFromGallery;
    }
   
    void OnDisable()
    {
        _Android.OnPickProfileImage -= OnPickImageFromGallery;
    }

    void OnPickImageFromGallery(Sprite obj)
    {
        _Profile._EditProfilePicTab.ProfilePicSprite = obj;
    }

    #region OnClickButton
    void OnClickButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == _UI.uploadImageButton)
            {
                _Android.OnPickGalleryImage();
            }
            if (button == _UI.confirmButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabEntity.GetEntityToken(GetEntity =>
                {
                    foreach (var item in GetEntity)
                    {
                        StartCoroutine(UploadProfileImageCoroutine(item.Key, item.Value));
                    }
                });
            }
            if (button == _UI.backButton)
            {
                MyCanvasGroups.CanvasGroupActivity(_Profile._EditProfilePicTab.EditProfileTabCanvasGroups, false);
            }
        }); 
    }
    #endregion

    #region UploadProfileImageCoroutine coroutine
    IEnumerator UploadProfileImageCoroutine(string entityId, string entityType)
    {
        _Profile.ProfileImage = _Profile._EditProfilePicTab.ProfilePicSprite;
        _Profile.ProfilePicContainer.DeleteCachedProfilePic(Photon.Pun.PhotonNetwork.LocalPlayer.UserId);
        yield return new WaitForSeconds(1);
        PlayerBaseConditions.PlayfabManager.PlayfabUploadProfileImage.UploadProfileImage(entityId, entityType, _Profile._EditProfilePicTab.ProfilePicSprite);
        MyCanvasGroups.CanvasGroupActivity(_Profile._EditProfilePicTab.EditProfileTabCanvasGroups, false);
    }
    #endregion
}
