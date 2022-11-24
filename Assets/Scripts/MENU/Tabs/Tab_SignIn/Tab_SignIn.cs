using UnityEngine;
using UnityEngine.UI;

public class Tab_SignIn : Tab_SignUp
{
    [SerializeField] private Toggle _toggleAutioSignIn;

    public bool IsAutoSignInChecked
    {
        get => _toggleAutioSignIn.isOn;
        set => _toggleAutioSignIn.isOn = value;
    }

    protected override CustomInputField CustomInputFieldID => _customInputFields[0];
    protected override CustomInputField CustomInputFieldPassword => _customInputFields[1];


    protected override void Authoirize()
    {
        if (_data.IsAutoSignInChecked)
        {
            CustomInputFieldID.Text = _data.Id;
            CustomInputFieldPassword.Text = _data.Password;
            IsAutoSignInChecked = true;
            base.OpenTab();
        }          
    }

    public override void Confirm()
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
