using UnityEngine;
using UnityEngine.UI;

public class Tab_SignIn : Tab_SignUp
{
    [SerializeField] private Toggle _toggleAutioSignIn;
    [SerializeField] private Toggle _toggleShowPassword; 

    public bool IsAutoSignInChecked
    {
        get => _toggleAutioSignIn.isOn;
        set => _toggleAutioSignIn.isOn = value;
    }
    public bool IsPasswordVisible
    {
        get => _toggleShowPassword.isOn;
        set => _toggleShowPassword.isOn = value;
    }


    protected override void Authoirize()
    {
        if (_data.IsAutoSignInChecked)
        {
            Id = _data.Id;
            Password = _data.Password;
            IsAutoSignInChecked = true;
            base.OpenTab();
        }          
    }

    public override void OnClickConfirmButton()
    {
        OnAutoSignChecked();
        base.OnClickConfirmButton();
    }

    protected override void OnEnter()
    {
        ExternalData.MyPlayfabRegistrationForm.Login(new MyPlayfabRegistrationValues { ID = Id, Password = Password, EMail = null });
    }

    private void OnAutoSignChecked()
    {
        if (IsAutoSignInChecked)
            _data.SetData(new Data.NewData { AutoSignIn = 1 });
        else
            _data.DeleteData(Keys.AutoSignIn);
    }

    protected override void SaveAccount()
    {
        if (IsAutoSignInChecked)
            base.SaveAccount();
    }

    public void OnToggleShowPasswordValueChanged()
    {
        if (IsPasswordVisible) _inputFieldPassword.contentType = InputField.ContentType.Standard;
        if (!IsPasswordVisible) _inputFieldPassword.contentType = InputField.ContentType.Password;

        _inputFieldPassword.Select();
    }
}
