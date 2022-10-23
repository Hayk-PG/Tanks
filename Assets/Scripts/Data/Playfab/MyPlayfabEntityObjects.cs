using PlayFab;
using PlayFab.DataModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;


public class MyPlayfabEntityObjects 
{
    public void Get(TitleProperties titleGroupProperties, Action<GetObjectsResponse> onResult)
    {
        GetObjectsRequest getObjectsRequest = new GetObjectsRequest();
        getObjectsRequest.Entity = new EntityKey { Id = titleGroupProperties.MemberID, Type = titleGroupProperties.MemberType };

        PlayFabDataAPI.GetObjects(getObjectsRequest, 
            onSuccess => 
            {
                onResult?.Invoke(onSuccess);
            }, 
            onError => 
            {
                onResult?.Invoke(null);
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }

    public void Set(TitleProperties titleGroupProperties, string objectName, Dictionary<string, string> dataObject, Action<SetObjectsResponse> onResult)
    {
        SetObjectsRequest setObjectsRequest = new SetObjectsRequest();
        setObjectsRequest.Entity = new EntityKey { Id = titleGroupProperties.MemberID, Type = titleGroupProperties.MemberType };
        List<SetObject> setObjects = new List<SetObject>() { new SetObject { ObjectName = objectName, DataObject = dataObject } };
        setObjectsRequest.Objects = setObjects;

        PlayFabDataAPI.SetObjects(setObjectsRequest, 
            onSuccess => 
            {
                onResult?.Invoke(onSuccess);
            }, 
            onError => 
            {
                onResult?.Invoke(null);
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }

    public void CheckEntityObject(TitleProperties titleGroupProperties, string objectName, string key, string value, Action<KeyValuePair<string, object>> onResult)
    {
        Get(titleGroupProperties, result => 
        {
            if (result != null)
            {
                foreach (var objectData in (JsonObject)result.Objects[objectName].DataObject)
                {
                    if (objectData.Key == key && (string)objectData.Value == value)
                    {
                        onResult?.Invoke(objectData);
                        break;
                    }
                }
            }
        });
    }
}
