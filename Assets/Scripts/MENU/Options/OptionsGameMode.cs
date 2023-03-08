using System;
using UnityEngine;

public class OptionsGameMode : OptionsController
{
    [SerializeField] 
    private Sprite _sprtOffline, _sprtOnline;

    public event Action onJumpTabSignUp;


    protected override void Select()
    {
        Conditions<bool>.Compare(!MyPhotonNetwork.IsOfflineMode, GoOffline, GoOnline);
    }

    private void GoOffline()
    {
        MyPhoton.Disconnect();

        OpenTabLoad();

        OnDisconnectCallback(delegate
        {
            MyPhotonNetwork.ManageOfflineMode(true);

            ChangeIcon(true);

            ChangeText(true);

            SetOptionsActivity(false);
        });
    }

    private void OnDisconnectCallback(Action action)
    {
        _options.MyPhotonCallbacks.onDisconnect -= delegate { action?.Invoke(); };
        _options.MyPhotonCallbacks.onDisconnect += delegate { action?.Invoke(); };
    }

    private void GoOnline()
    {
        onJumpTabSignUp?.Invoke();

        OpenTabLoad();     
    }

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
