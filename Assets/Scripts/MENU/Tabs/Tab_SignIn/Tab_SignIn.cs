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
        //base.OnPhotonConnectedToMaster();
    }
}
