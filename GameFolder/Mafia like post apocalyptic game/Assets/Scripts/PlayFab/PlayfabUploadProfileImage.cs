using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.DataModels;
using PlayFab.Internal;

public class PlayfabUploadProfileImage : MonoBehaviour
{
    List<string> FilesList = new List<string>() { PlayerKeys.ProfilePicture };

    #region UploadProfileImage
    public void UploadProfileImage(string entityId, string entityType)
    {
        InitiateFileUploadsRequest requestFileUpload = new InitiateFileUploadsRequest();

        requestFileUpload.Entity = new EntityKey();
        requestFileUpload.Entity.Id = entityId;
        requestFileUpload.Entity.Type = entityType;

        requestFileUpload.FileNames = FilesList;

        PlayFabDataAPI.InitiateFileUploads(requestFileUpload,
            upload =>
            {
                Texture2D avatarTexture = FindObjectOfType<UploadImage>()._UI.ProfilePic.texture;

                byte[] bytes = avatarTexture.EncodeToPNG();

                PlayFabHttp.SimplePutCall(upload.UploadDetails[0].UploadUrl, bytes,
                    succed =>
                    {
                        print(succed);
                        FinalizeUploadFile(entityId, entityType);
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
    void FinalizeUploadFile(string entityId, string entityType)
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
}
