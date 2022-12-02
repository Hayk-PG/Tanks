using System;
using UnityEngine;

public class OptionsGameMode : OptionsController
{
    private MyPhotonCallbacks _myPhotonCallbacks;
    [SerializeField] private Sprite _sprtOffline, _sprtOnline;

    public event Action onSelectOnlineMode;


    protected override void Awake()
    {
        base.Awake();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected override void Select()
    {
        Conditions<bool>.Compare(!MyPhotonNetwork.IsOfflineMode, Disconnect, GoThroughSignUpTab);
    }

    protected override void GetOptionsActivity(bool isActive)
    {
        _myPhotonCallbacks.onDisconnect -= GoOffline;
        _myPhotonCallbacks._OnConnectedToMaster -= GoOnline;
    }

    private void Disconnect()
    {
        MyPhoton.Disconnect();
        _myPhotonCallbacks.onDisconnect += GoOffline;
    }

    private void GoOffline()
    {
        ChangeIcon(true);
        MyPhotonNetwork.OfflineMode(true);
        _options.Activity(false);
    }

    private void GoThroughSignUpTab()
    {
        MyPhotonNetwork.OfflineMode(false);
        onSelectOnlineMode?.Invoke();
        _myPhotonCallbacks._OnConnectedToMaster += GoOnline;       
    }

    private void GoOnline()
    {
        ChangeIcon(false);
        _options.Activity(false);
    }

    private void ChangeIcon(bool isOffline)
    {
        _icon.sprite = isOffline ? _sprtOnline : _sprtOffline;
    }

    public override void SetDefault()
    {
        ChangeIcon(MyPhotonNetwork.IsOfflineMode);
    }
}
