using System;
using UnityEngine;

public class Tab_StartGame : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private Btn _btnOffline;
    [SerializeField] private Btn _btnOnline;

    public event Action onPlayOffline;
    public event Action onPlayOnline;


    protected override void OnEnable()
    {
        _btnOffline.onSelect += SelectOffline;
        _btnOnline.onSelect += SelectOnline;
    }

    protected override void OnDisable()
    {
        _btnOffline.onSelect -= SelectOffline;
        _btnOnline.onSelect -= SelectOnline;
    }
   
    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();
        MyPhoton.Disconnect();

        base.OpenTab();
    }

    public void SelectOffline()
    {
        MyPhotonNetwork.OfflineMode(true);
        onPlayOffline?.Invoke();
    }

    public void SelectOnline()
    {
        MyPhotonNetwork.OfflineMode(false);
        onPlayOnline?.Invoke();
    }
}
