using System;

public class OptionsLogOut : OptionsController
{
    public event Action onJumpTabSignUp;



    protected override void OnEnable()
    {
        base.OnEnable();
        _myPhotonCallbacks.onDisconnect += OpenTabSignUp;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _myPhotonCallbacks.onDisconnect -= OpenTabSignUp;
    }

    protected override void Select()
    {     
        OpenTabLoad();
        MyPhoton.Disconnect();
        _isSelected = true;
    }

    private void OpenTabSignUp()
    {
        if (_isSelected)
        {
            MyPhotonNetwork.OfflineMode(false);
            Data.Manager.DeleteData(Keys.AutoSignIn);
            SetOptionsActivity(false);
            onJumpTabSignUp?.Invoke();
            _isSelected = false;
        }
    }
}