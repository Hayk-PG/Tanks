using PlayFab;
using PlayFab.DataModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;


public class MyPlayfabEntityObjects 
{
    private Dictionary<string, object> DataObjectDict { get; set; }


    private void Get(TitleProperties titleGroupProperties, Action<GetObjectsResponse> onResult)
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

    public void Get(TitleProperties titleGroupProperties, string objectName, Action<Dictionary<string, object>> onResult)
    {
        Get(titleGroupProperties, result =>
        {
            if (result != null && result.Objects.ContainsKey(objectName))
            {
                DataObjectDict = new Dictionary<string, object>();

                foreach (var objectData in (JsonObject)result.Objects[objectName].DataObject)
                {
                    DataObjectDict.Add(objectData.Key, objectData.Value);
                }

                onResult?.Invoke(DataObjectDict);
            }
        });
    }

    public void Set(TitleProperties titleProperties, string objectName, Dictionary<string, string> dataObject, Action<SetObjectsResponse> onResult)
    {
        SetObjectsRequest setObjectsRequest = new SetObjectsRequest();
        setObjectsRequest.Entity = new EntityKey { Id = titleProperties.MemberID, Type = titleProperties.MemberType };
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

    public void Delete(TitleProperties titleProperties, string objectName, Action<SetObjectsResponse> onResult)
    {
        SetObjectsRequest setObjectsRequest = new SetObjectsRequest();
        setObjectsRequest.Entity = new EntityKey { Id = titleProperties.MemberID, Type = titleProperties.MemberType };
        List<SetObject> setObjects = new List<SetObject>() { new SetObject { ObjectName = objectName, DeleteObject = true } };
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
}
