using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public partial class MyPlayfab : MonoBehaviour
{
    public void Login(RegistrationData registrationData)
    {
        LoginWithPlayFabRequest lwpr = new LoginWithPlayFabRequest();
        lwpr.Username = registrationData._userName;
        lwpr.Password = registrationData._password;

        PlayFabClientAPI.LoginWithPlayFab(lwpr, 
            onSucces => 
            {
                Data.Manager.PlayfabId = onSucces.PlayFabId;
                Data.Manager.EntityID = onSucces.EntityToken.Entity.Id;
                Data.Manager.EntityType = onSucces.EntityToken.Entity.Type;

                MyPhoton.SetNickName(registrationData._userName);
                OnGetReadOnlyData?.Invoke(onSucces.PlayFabId, GetUserReadOnlyData);                
            }, 
            onError => 
            {
                foreach (var error in onError.ErrorDetails)
                {
                    print(error);
                }
            });
    }
}
