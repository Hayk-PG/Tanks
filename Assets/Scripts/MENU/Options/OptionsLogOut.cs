
public class OptionsLogOut : OptionsController
{
    protected override void Select()
    {     
        OpenTabLoad(15);

        _isSelected = true;

        if (MyPhotonNetwork.IsConnected)
        {
            MyPhoton.Disconnect();

            ManageMyOnDisconnectEventSubscription(false);
            ManageMyOnDisconnectEventSubscription(true);

            return;
        }

        OpenTabSignUp();
    }

    private void ManageMyOnDisconnectEventSubscription(bool isSubscribing)
    {
        if(isSubscribing)
            _options.MyPhotonCallbacks.onDisconnect += OpenTabSignUp;
        else
            _options.MyPhotonCallbacks.onDisconnect -= OpenTabSignUp;
    }

    private void OpenTabSignUp()
    {
        if (_isSelected)
        {
            MyPhotonNetwork.ManageOfflineMode(false);

            Data.Manager.DeleteData(Keys.AutoSignIn);

            SetOptionsActivity(false);

            TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Authenticate);;

            ManageMyOnDisconnectEventSubscription(false);

            _isSelected = false;
        }
    }

    public override void OnOperationSucceded()
    {
        _tabLoading.Close();

        SetOptionsActivity(false);
    }

    public override void OnOperationFailed() => _tabLoading.Close();
}
