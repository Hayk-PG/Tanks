using PlayFab;
using PlayFab.ClientModels;
using System;

public class MyPlayfabProfile 
{
    public void Get(string playfabId, Action<GetPlayerProfileResult> onResult)
    {
        GetPlayerProfileRequest getPlayerProfileRequest = new GetPlayerProfileRequest();
        getPlayerProfileRequest.PlayFabId = playfabId;
        getPlayerProfileRequest.ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true};

        PlayFabClientAPI.GetPlayerProfile(getPlayerProfileRequest, 
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
