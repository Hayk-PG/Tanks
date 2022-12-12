using System;
using System.Collections.Generic;
using PlayFab;

public class UserData 
{
    /// <summary>
    /// Get
    /// </summary>
    /// <param name="playfabId"></param>
    /// <param name="onResult"></param>
    public UserData(string playfabId, Action<Dictionary<string, PlayFab.ClientModels.UserDataRecord>> onResult)
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

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="playfabId"></param>
    /// <param name="permission"></param>
    /// <param name="newData"></param>
    /// <param name="keysToRemove"></param>
    public UserData(string playfabId, PlayFab.ServerModels.UserDataPermission permission, Dictionary<string, string> newData, List<string> keysToRemove)
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
                
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }
}
