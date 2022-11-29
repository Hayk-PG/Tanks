using System;
using UnityEngine;

public class Tab_SignUp : Tab_BaseSignUp
{
    [SerializeField] private Btn _btnSignIn;

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

    protected override void Authoirize()
    {
        if (!_data.IsAutoSignInChecked)
            OpenTab();
    }

    protected override void Confirm()
    {
        User.Register(CustomInputFieldID.Text, CustomInputFieldPassword.Text, CustomInputFieldEmail.Text, result =>
        {
            if (result != null)
            {
                SaveUserCredentials(NewData(CustomInputFieldID.Text, CustomInputFieldPassword.Text));
                CacheUserIds(result.PlayFabId, result.EntityToken.Entity.Id, result.EntityToken.Entity.Type);
                CreateUserItemsData(result.PlayFabId);
                ConnectToPhoton(CustomInputFieldID.Text, result.PlayFabId);
            }
        });
    }
}
