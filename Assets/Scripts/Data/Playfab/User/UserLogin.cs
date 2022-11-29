using PlayFab;
using PlayFab.ClientModels;
using System;

public class UserLogin 
{
    public UserLogin(string userId, string userPassword, Action<LoginResult> onResult)
    {
        LoginWithPlayFabRequest loginWithPlayFabRequest = new LoginWithPlayFabRequest();
        loginWithPlayFabRequest.Username = userId;
        loginWithPlayFabRequest.Password = userPassword;

        PlayFabClientAPI.LoginWithPlayFab(loginWithPlayFabRequest,
            onSucces =>
            {
                onResult?.Invoke(onSucces);
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
                onResult?.Invoke(null);
            });
    }
}
