using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_SignIn : Tab_BaseSignUp
{
    [SerializeField] private Toggle _toggleAutioSignIn;
    [SerializeField] private Btn _btnSignUp;

    public bool IsAutoSignInChecked
    {
        get => _toggleAutioSignIn.isOn;
        set => _toggleAutioSignIn.isOn = value;
    }

    protected override CustomInputField CustomInputFieldID => _customInputFields[0];
    protected override CustomInputField CustomInputFieldPassword => _customInputFields[1];

    public event Action onOpenTabSignUp;


    protected override void Awake()
    {
        base.Awake();
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
        if (_data.IsAutoSignInChecked)
        {
            CustomInputFieldID.Text = _data.Id;
            CustomInputFieldPassword.Text = _data.Password;
            IsAutoSignInChecked = true;
            Confirm();
        }
    }

    protected override void Confirm()
    {
        MyPlayfabRegistrationValues myPlayfabRegistrationValues = new MyPlayfabRegistrationValues
        {
            ID = CustomInputFieldID.Text,
            Password = CustomInputFieldPassword.Text
        };

        ExternalData.MyPlayfabRegistrationForm.Login(myPlayfabRegistrationValues, result =>
        {
            OnAutoSignChecked();
            SaveData(NewData(myPlayfabRegistrationValues));
        });
    }

    private void OnAutoSignChecked()
    {
        if (IsAutoSignInChecked)
            _data.SetData(new Data.NewData { AutoSignIn = 1 });
        else
            _data.DeleteData(Keys.AutoSignIn);
    }

    protected override void SaveData(Data.NewData newData)
    {
        if (IsAutoSignInChecked)
            base.SaveData(newData);
    }
}
