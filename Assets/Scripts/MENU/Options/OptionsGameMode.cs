using UnityEngine;

public class OptionsGameMode : OptionsController
{
    [SerializeField] 
    private Sprite _sprtOffline, _sprtOnline;



    protected override void Select() => Conditions<bool>.Compare(!MyPhotonNetwork.IsOfflineMode, GoOffline, GoOnline);

    private void GoOffline()
    {
        OpenTabLoad();

        if (MyPhotonNetwork.IsConnected)
        {
            MyPhoton.Disconnect();

            _options.MyPhotonCallbacks.onDisconnect -= delegate { TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline); };
            _options.MyPhotonCallbacks.onDisconnect += delegate { TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline); };

            return;
        }

        TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline);
    }

    private void GoOnline()
    {
        OpenTabLoad();

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
