using PlayFab;
using PlayFab.ClientModels;
using System;

public class UserRegister 
{
    public UserRegister(string userId, string userPassword, string userEmail, Action<RegisterPlayFabUserResult> onResult)
    {
        RegisterPlayFabUserRequest registerPlayFabUserRequest = new RegisterPlayFabUserRequest();
        registerPlayFabUserRequest.Username = userId;
        registerPlayFabUserRequest.DisplayName = userId;
        registerPlayFabUserRequest.Password = userPassword;

        if (!String.IsNullOrEmpty(userEmail))
            registerPlayFabUserRequest.Email = userEmail;

        registerPlayFabUserRequest.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(registerPlayFabUserRequest,
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
