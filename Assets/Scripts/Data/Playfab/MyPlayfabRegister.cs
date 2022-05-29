using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public partial class MyPlayfab : MonoBehaviour
{    
    public struct RegistrationData
    {
        public string _userName;
        public string _password;
        public string _email;

        public RegistrationData(string userName, string password, string email)
        {
            _userName = userName;
            _password = password;
            _email = email;
        }
    }

    public void Register(RegistrationData registrationData)
    {
        RegisterPlayFabUserRequest rpfur = new RegisterPlayFabUserRequest();
        rpfur.Username = registrationData._userName;
        rpfur.DisplayName = registrationData._userName;
        rpfur.Password = registrationData._password;
        if (!String.IsNullOrEmpty(registrationData._email)) rpfur.Email = registrationData._email;
        rpfur.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(rpfur, 
            onSucces => 
            {
                MyPhoton.SetNickName(registrationData._userName);
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
