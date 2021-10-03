using PlayFab;
using PlayFab.DataModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabJsonObjects : MonoBehaviour
{
    public void SetObjects(string entityId, string entityType, string ObjectName, Dictionary<string, object> DataObject)
    {
        SetObjectsRequest setObjectsRequest = new SetObjectsRequest();
        setObjectsRequest.Entity = new EntityKey { Id = entityId, Type = entityType };
        setObjectsRequest.Objects = new List<SetObject> { new SetObject { ObjectName = ObjectName != null ? ObjectName: PlayerKeys.PlayfabObjectDefaultKey, DataObject = DataObject } };

        PlayFabDataAPI.SetObjects(setObjectsRequest, 
            set => 
            {
                print("Object set successfuly");
            }, 
            error => 
            {
                print("Failed to set Object");
            });
    }

    public void GetObjects(string entityId, string entityType, string Key, Action<GetObjectsResponse> Object)
    {
        GetObjectsRequest getObjectsRequest = new GetObjectsRequest();
        getObjectsRequest.Entity = new EntityKey { Id = entityId, Type = entityType };

        PlayFabDataAPI.GetObjects(getObjectsRequest, 
            get => 
            {
                if (get.Objects.ContainsKey(Key))
                {
                    Object?.Invoke(get);
                }
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
}
