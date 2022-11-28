using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using System;

public class MyPlayfabUserData
{
    public void Get(string playfabId, Action<Dictionary<string, PlayFab.ClientModels.UserDataRecord>> onResult)
    {
        PlayFab.ClientModels.GetUserDataRequest getUserDataRequest = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = playfabId
        };

        PlayFabClientAPI.GetUserData(getUserDataRequest,
            onSuccess =>
            {
                onResult?.Invoke(onSuccess.Data);
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
                onResult?.Invoke(null);
            });
    }

    public void Update(string playfabId, PlayFab.ServerModels.UserDataPermission permission, Dictionary<string,  string> newData, List<string> keysToRemove)
    {
        PlayFab.ServerModels.UpdateUserDataRequest updateUserDataRequest = new PlayFab.ServerModels.UpdateUserDataRequest
        {
            PlayFabId = playfabId,
            Data = newData,
            KeysToRemove = keysToRemove,
            Permission = permission
        };

        PlayFabServerAPI.UpdateUserData(updateUserDataRequest,
            onSuccess =>
            {
                GlobalFunctions.DebugLog("User data has been updated");
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }
}

