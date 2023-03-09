using System;
using System.Collections;
using UnityEngine;

public class Tab_StartGame : Tab_Base, ITabOperation
{
    [SerializeField]
    private Btn _btnOffline, _btnOnline;

    public event Action onPlayOffline;


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
        StartCoroutine(Execute(true, onPlayOffline));
    }

    public void SelectOnline()
    {
        StartCoroutine(Execute(false, delegate { TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Authenticate); }));
    }

    private IEnumerator Execute(bool isOfflineMode, Action onPlay)
    {
        _tabLoading.Open();

        yield return new WaitForSeconds(0.5f);

        MyPhotonNetwork.ManageOfflineMode(isOfflineMode);

        onPlay?.Invoke();
    }

    public override void OnOperationFailed()
    {
        print("Can't authorize");

        _tabLoading.Close();
    }

    public override void OnOperationSucceded()
    {
        
    }
}
