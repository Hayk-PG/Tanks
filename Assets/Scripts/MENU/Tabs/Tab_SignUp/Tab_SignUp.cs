using System;
using UnityEngine;

public class Tab_SignUp : Tab_BaseSignUp
{
    [SerializeField] [Space]
    private Btn _btnSignIn;

    protected override CustomInputField CustomInputFieldEmail => _customInputFields[0];
    protected override CustomInputField CustomInputFieldID => _customInputFields[1];
    protected override CustomInputField CustomInputFieldPassword => _customInputFields[2];

    public event Action onOpenTabSignIn;



    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_SignIn.onOpenTabSignUp += OpenTab;

        _btnSignIn.onSelect += delegate { onOpenTabSignIn?.Invoke(); };      
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        MenuTabs.Tab_SignIn.onOpenTabSignUp -= OpenTab;

        _btnSignIn.onSelect -= delegate { onOpenTabSignIn?.Invoke(); };      
    }

    protected override void Authenticate()
    {
        if (!Data.Manager.IsAutoSignInChecked)
            OpenTab();
    }

    protected override void Confirm()
    {
        base.Confirm();

        User.Register(CustomInputFieldID.Text, CustomInputFieldPassword.Text, CustomInputFieldEmail.Text, result =>
        {
            if (result != null)
            {
                MyPhotonNetwork.ManageOfflineMode(false);

                SaveUserCredentials(NewData(CustomInputFieldID.Text, CustomInputFieldPassword.Text));

                CacheUserIds(result.PlayFabId, result.EntityToken.Entity.Id, result.EntityToken.Entity.Type);

                CacheUserItemsData(result.PlayFabId);

                CacheUserStatisticsData(result.PlayFabId);

                SendUserCredentialsToTabHomeOnline(CustomInputFieldID.Text, result.PlayFabId);
            }
            else
            {
                ResetTab();

                OnAuthenticationFailed();
            }
        });
    }

    public override void OnOperationFailed()
    {
        print("Failed to sign up! " + OperationHandler);

        if (OperationHandler == This)
        {
            base.OpenTab();
        }
        else
        {
            OperationHandler.OnOperationFailed();
        }
    }
}
