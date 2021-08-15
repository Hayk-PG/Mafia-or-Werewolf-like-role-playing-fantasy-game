using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.Internal;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using System;

public class PlayfabFile : MonoBehaviour
{
    List<string> FilesList = new List<string>() { PlayerKeys.ProfilePicture };

    [SerializeField] Sprite spriteImage;

    [SerializeField] bool delete;
    [SerializeField] bool upload;
    [SerializeField] bool finish;
    [SerializeField] bool abort;
    [SerializeField] bool get;

    [SerializeField] string url;

    [SerializeField] SpriteRenderer sp;
    [SerializeField] Sprite aaa;

    string entityId
    {
        get => PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.EntityId].ToString();
    }
    string entityType
    {
        get => PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.EntityType].ToString();
    }


    void Update()
    {      
        if (delete)
        {
            DeleteFile();
            delete = false;
        }

        if (upload)
        {
            UploadFile();
            upload = false;
        }
        if (finish)
        {
            FinalizeUploadFile();
            finish = false;
        }

        if (abort)
        {
            AbortUploading();
            abort = false;
        }

        if (get)
        {
            //GetPlayfabFile();
            get = false;
        }
    }
   
    #region DeleteFile
    void DeleteFile()
    {
        DeleteFilesRequest requestDeleteFile = new DeleteFilesRequest();
        requestDeleteFile.Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType };
        requestDeleteFile.FileNames = FilesList;

        PlayFabDataAPI.DeleteFiles(requestDeleteFile,
            delete =>
            {
                UploadFile();
            },
            error =>
            {
                if(error.Error == PlayFabErrorCode.FileNotFound) UploadFile();

                print(error.ErrorMessage);
            });
    }
    #endregion

    #region UploadFile
    void UploadFile()
    {
        InitiateFileUploadsRequest requestFileUpload = new InitiateFileUploadsRequest();

        requestFileUpload.Entity = new PlayFab.DataModels.EntityKey();
        requestFileUpload.Entity.Id = entityId;
        requestFileUpload.Entity.Type = entityType;

        requestFileUpload.FileNames = FilesList;

        PlayFabDataAPI.InitiateFileUploads(requestFileUpload,
            upload =>
            {
                Texture2D avatarTexture = spriteImage.texture;

                byte[] bytes = avatarTexture.EncodeToPNG();

                PlayFabHttp.SimplePutCall(upload.UploadDetails[0].UploadUrl, bytes, 
                    succed => 
                    {
                        print(succed);
                        FinalizeUploadFile();
                    }, 
                    error => 
                    {
                        print(error);
                    });
            },
            error =>
            {                
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region FinalizeUploadFile
    void FinalizeUploadFile()
    {
        FinalizeFileUploadsRequest finalizeFileUpload = new FinalizeFileUploadsRequest();
        finalizeFileUpload.Entity = new PlayFab.DataModels.EntityKey();
        finalizeFileUpload.Entity.Id = entityId;
        finalizeFileUpload.Entity.Type = entityType;
        finalizeFileUpload.FileNames = FilesList;

        PlayFabDataAPI.FinalizeFileUploads(finalizeFileUpload, 
            finalize => 
            {
                
            }, 
            error => 
            {
                print(error.HttpStatus);
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region AbortUploading
    void AbortUploading()
    {
        AbortFileUploadsRequest requestAbortUploading = new AbortFileUploadsRequest();
        requestAbortUploading.Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType };
        requestAbortUploading.FileNames = FilesList;

        PlayFabDataAPI.AbortFileUploads(requestAbortUploading, 
            abort => 
            {
                print("ABORTED");
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region GetFile
    public void GetPlayfabFile(string entityId, string entityType, Action<string> GetProfilePictureURL)
    {
        GetFilesRequest getFile = new GetFilesRequest();
        getFile.Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType };

        print(entityId + "/" + entityType);

        PlayFabDataAPI.GetFiles(getFile, 
            get => 
            {
                foreach (var item in get.Metadata)
                {
                    if(item.Value.FileName == PlayerKeys.ProfilePicture)
                    {
                        GetProfilePictureURL(item.Value.DownloadUrl);
                    }
                }
            }, 
            error => 
            {
                print(error.ErrorMessage);
                print(error.Error);
            });
    }
    #endregion

    #region UpdateAvatarURL
    void UpdateAvatarURL(string avatarURL)
    {       
        UpdateAvatarUrlRequest requestUpdateURL = new UpdateAvatarUrlRequest();
        requestUpdateURL.ImageUrl = avatarURL;

        PlayFabClientAPI.UpdateAvatarUrl(requestUpdateURL, 
            update => 
            {
                print("SUCCED");
                //GetAvatar(avatarURL);
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region GetAvatar
    void GetAvatar(string avatarURL)
    {
        //PlayFabHttp.SimpleGetCall(avatarURL, 
        //    succed => 
        //    {
        //        StartCoroutine(Test(avatarURL))
        //    }, 
        //    error => 
        //    {
        //        print(error);
        //    });
        StartCoroutine(ShowAvatarCoroutine(avatarURL));
    }
    #endregion

    #region ShowAvatarCoroutine
    IEnumerator ShowAvatarCoroutine(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        Texture2D downloadedAvatar = ((DownloadHandlerTexture)request.downloadHandler).texture;
        sp.sprite = Sprite.Create(downloadedAvatar, new Rect(0, 0, downloadedAvatar.width, downloadedAvatar.height), new Vector2(0.5f, 0.5f), 100);
    }
    #endregion
}
