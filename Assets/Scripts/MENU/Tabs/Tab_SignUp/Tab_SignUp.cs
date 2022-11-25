﻿using System;
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
        MyPlayfabRegistrationValues myPlayfabRegistrationValues = new MyPlayfabRegistrationValues
        {
            EMail = CustomInputFieldEmail.Text,
            ID = CustomInputFieldID.Text,
            Password = CustomInputFieldPassword.Text
        };

        ExternalData.MyPlayfabRegistrationForm.Register(myPlayfabRegistrationValues, result =>
        {
            SaveData(NewData(myPlayfabRegistrationValues));
        });
    }
}
