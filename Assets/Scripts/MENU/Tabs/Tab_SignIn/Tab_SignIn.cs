﻿using System;
using UnityEngine;

public class Tab_SignIn : Tab_BaseSignUp
{
    [SerializeField] private Btn _btnSignUp;
    private CustomToggle _customToggleAutoSignIn;

    protected override CustomInputField CustomInputFieldID => _customInputFields[0];
    protected override CustomInputField CustomInputFieldPassword => _customInputFields[1];
    public bool IsAutoSignInChecked { get => _customToggleAutoSignIn.IsOn; set => _customToggleAutoSignIn.IsOn = value; }

    public event Action onOpenTabSignUp;



    protected override void Awake()
    {
        base.Awake();
        _customToggleAutoSignIn = Get<CustomToggle>.FromChild(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MenuTabs.Tab_SignUp.onOpenTabSignIn += OpenTab;
        _btnSignUp.onSelect += delegate { onOpenTabSignUp?.Invoke(); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MenuTabs.Tab_SignUp.onOpenTabSignIn -= OpenTab;
        _btnSignUp.onSelect -= delegate { onOpenTabSignUp?.Invoke(); };
    }

    protected override void Authoirize()
    {
        if (Data.Manager.IsAutoSignInChecked)
        {
            CustomInputFieldID.Text = Data.Manager.Id;
            CustomInputFieldPassword.Text = Data.Manager.Password;
            IsAutoSignInChecked = true;
            Confirm();
        }
    }

    protected override void Confirm()
    {
        base.Confirm();

        User.Login(CustomInputFieldID.Text, CustomInputFieldPassword.Text, result =>
        {
            if (result != null)
            {
                DeleteOrEnableAutoSignInData(); 
                SaveUserCredentials(NewData(CustomInputFieldID.Text, CustomInputFieldPassword.Text));
                CacheUserIds(result.PlayFabId, result.EntityToken.Entity.Id, result.EntityToken.Entity.Type);
                CreateUserItemsData(result.PlayFabId);
                ConnectToPhoton(CustomInputFieldID.Text, result.PlayFabId);
            }
        });
    }

    private void DeleteOrEnableAutoSignInData()
    {
        if (IsAutoSignInChecked)
            Data.Manager.SetData(new Data.NewData { AutoSignIn = 1 });
        else
            Data.Manager.DeleteData(Keys.AutoSignIn);
    }

    protected override void SaveUserCredentials(Data.NewData newData)
    {
        if (IsAutoSignInChecked)
            base.SaveUserCredentials(newData);
    }
}
