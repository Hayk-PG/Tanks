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
                MyPhoton.SetNickName(registrationData._userName);
                OnUpdateReadOnlyData?.Invoke(onSucces.PlayFabId, UpdateUserDataRequest);
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
