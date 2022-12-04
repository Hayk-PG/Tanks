using System;
using System.Collections;
using UnityEngine;

public class Tab_StartGame : Tab_Base
{
    [SerializeField] private Btn _btnOffline;
    [SerializeField] private Btn _btnOnline;
    [SerializeField] private TabLoading _tabLoading;

    public event Action onPlayOffline;
    public event Action onPlayOnline;


    protected override void OnEnable()
    {
        MenuTabs.Tab_Initialize.onOpenTabStartGame += OpenTab;
        _btnOffline.onSelect += SelectOffline;
        _btnOnline.onSelect += SelectOnline;
    }

    protected override void OnDisable()
    {
        MenuTabs.Tab_Initialize.onOpenTabStartGame -= OpenTab;
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
        _tabLoading.Open();
        StartCoroutine(OfflineCoroutine());
    }

    public void SelectOnline()
    {
        _tabLoading.Open();
        MyPhotonNetwork.OfflineMode(false);
        onPlayOnline?.Invoke();
    }

    private IEnumerator OfflineCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        MyPhotonNetwork.OfflineMode(true);
        onPlayOffline?.Invoke();
    }
}
