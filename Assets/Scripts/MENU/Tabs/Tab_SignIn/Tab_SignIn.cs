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


    protected override void OnPhotonConnectedToMaster()
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
        if(Id == _data.Id && Password == _data.Password)
        {
            OnAutoSignChecked();
            base.OnClickConfirmButton();
        }  
    }

    private void OnAutoSignChecked()
    {
        if (IsAutoSignInChecked)
            _data.SetData(new Data.NewData { AutoSignIn = 1 });
        else
            _data.DeleteData(Keys.AutoSignIn);
    }
}
