using PlayFab;
using PlayFab.ClientModels;
using System;

public struct MyPlayfabRegistrationValues
{
    public string ID { get; set; }
    public string Password { get; set; }
    public string EMail { get; set; }
}

public class MyPlayfabRegistrationForm 
{
    private struct Result
    {
        internal string PlayfabID { get; set; }
        internal string EntityID { get; set; }
        internal string EntityType { get; set; }
    }

    public event Action onLogin;

    public void Register(MyPlayfabRegistrationValues authValues, Action<bool> onResult)
    {
        RegisterPlayFabUserRequest rpfur = new RegisterPlayFabUserRequest();
        rpfur.Username = authValues.ID;
        rpfur.DisplayName = authValues.ID;
        rpfur.Password = authValues.Password;

        if (!String.IsNullOrEmpty(authValues.EMail))
            rpfur.Email = authValues.EMail;

        rpfur.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(rpfur,
            onSucces =>
            {
                StorePlayerIds(new Result { PlayfabID = onSucces.PlayFabId, EntityID = onSucces.EntityToken.Entity.Id, EntityType = onSucces.EntityToken.Entity.Type });
                MyPhoton.Connect(new MyPhotonAuthValues { Nickname = authValues.ID, UserID = onSucces.PlayFabId });
                onResult?.Invoke(true);
                onLogin.Invoke();
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
                onResult?.Invoke(false);
            });
    }

    public void Login(MyPlayfabRegistrationValues authValues, Action<bool> onResult)
    {
        LoginWithPlayFabRequest lwpr = new LoginWithPlayFabRequest();
        lwpr.Username = authValues.ID;
        lwpr.Password = authValues.Password;

        PlayFabClientAPI.LoginWithPlayFab(lwpr,
            onSucces =>
            {
                StorePlayerIds(new Result { PlayfabID = onSucces.PlayFabId, EntityID = onSucces.EntityToken.Entity.Id, EntityType = onSucces.EntityToken.Entity.Type });
                MyPhoton.Connect(new MyPhotonAuthValues { Nickname = authValues.ID, UserID = onSucces.PlayFabId });
                onResult?.Invoke(true);
                onLogin.Invoke();
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
                onResult?.Invoke(false);
            });
    }

    private void StorePlayerIds(Result result)
    {
        Data.Manager.PlayfabId = result.PlayfabID;
        Data.Manager.EntityID = result.EntityID;
        Data.Manager.EntityType = result.EntityType;
    }
}
