using UnityEngine;

public class OptionsGameMode : OptionsController
{
    [SerializeField] 
    private Sprite _sprtOffline, _sprtOnline;



    protected override void Select() => Conditions<bool>.Compare(!MyPhotonNetwork.IsOfflineMode, GoOffline, GoOnline);

    private void GoOffline()
    {
        OpenTabLoad(5);

        if (MyPhotonNetwork.IsConnected)
        {
            MyPhoton.Disconnect();

            ManageOnDisconnectEventSubscription(false);
            ManageOnDisconnectEventSubscription(true);

            return;
        }

        TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline);
    }

    private void ManageOnDisconnectEventSubscription(bool isSubscribing)
    {
        if (isSubscribing)
            _options.MyPhotonCallbacks.onDisconnect += OnDisconnect;
        else
            _options.MyPhotonCallbacks.onDisconnect -= OnDisconnect;
    }

    private void OnDisconnect()
    {
        TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline);

        ManageOnDisconnectEventSubscription(false);
    }

    private void GoOnline()
    {
        OpenTabLoad(10);

        TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Authenticate);
    }

    public override void OnOperationSucceded()
    {
        _tabLoading.Close();

        SetOptionsActivity(false);
    }

    public override void OnOperationFailed() => _tabLoading.Close();

    public override void SetDefault()
    {
        ChangeIcon(MyPhotonNetwork.IsOfflineMode);

        ChangeText(MyPhotonNetwork.IsOfflineMode);
    }

    private void ChangeIcon(bool isOffline) => _icon.sprite = isOffline ? _sprtOnline : _sprtOffline;

    private void ChangeText(bool isOffline)
    {
        _btnTxt.SetButtonTitle(isOffline ? GlobalFunctions.TextWithColorCode("#1EFDB6", "go online") : GlobalFunctions.TextWithColorCode("#FD1E40", "go offline"));
    }
}
