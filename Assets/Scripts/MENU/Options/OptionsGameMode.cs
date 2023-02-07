using System;
using UnityEngine;

public class OptionsGameMode : OptionsController
{
    [SerializeField] private Sprite _sprtOffline, _sprtOnline;

    public event Action onJumpTabSignUp;


    protected override void Select()
    {
        Conditions<bool>.Compare(!MyPhotonNetwork.IsOfflineMode, Disconnect, GoThroughSignUpTab);
    }

    protected override void GetOptionsActivity(bool isActive)
    {
        _options.MyPhotonCallbacks.onDisconnect -= GoOffline;
        _options.MyPhotonCallbacks._OnConnectedToMaster -= FinishGoingOnline;
    }

    private void Disconnect()
    {      
        MyPhoton.Disconnect();
        OpenTabLoad();
        _options.MyPhotonCallbacks.onDisconnect += GoOffline;
    }

    private void GoOffline()
    {
        MyPhotonNetwork.OfflineMode(true);
        ChangeIcon(true);
        SetOptionsActivity(false);
    }

    private void GoThroughSignUpTab()
    {       
        MyPhotonNetwork.OfflineMode(false);
        OpenTabLoad();
        onJumpTabSignUp?.Invoke();
        _options.MyPhotonCallbacks._OnConnectedToMaster += FinishGoingOnline;       
    }

    private void FinishGoingOnline()
    {
        ChangeIcon(false);
        SetOptionsActivity(false);
    }

    private void ChangeIcon(bool isOffline) => _icon.sprite = isOffline ? _sprtOnline : _sprtOffline;

    public override void SetDefault() => ChangeIcon(MyPhotonNetwork.IsOfflineMode);
}
