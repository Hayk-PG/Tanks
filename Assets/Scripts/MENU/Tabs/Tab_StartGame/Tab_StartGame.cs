using System;
using System.Collections;
using UnityEngine;

public class Tab_StartGame : Tab_Base, ITabOperation
{
    [SerializeField]
    private Btn _btnOffline, _btnOnline;



    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_SignUp.onGoBack += OpenTab;
        MenuTabs.Tab_SignIn.onGoBack += OpenTab;

        _btnOffline.onSelect += SelectOffline;

        _btnOnline.onSelect += SelectOnline;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        MenuTabs.Tab_SignUp.onGoBack -= OpenTab;
        MenuTabs.Tab_SignIn.onGoBack -= OpenTab;

        _btnOffline.onSelect -= SelectOffline;

        _btnOnline.onSelect -= SelectOnline;
    }

    protected override void OnOperationSubmitted(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if (operation == TabsOperation.Operation.Start)
        {
            OperationHandler = handler;

            OpenTab();
        }
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
        StartCoroutine(Execute(delegate { TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline); }));
    }

    public void SelectOnline()
    {
        StartCoroutine(Execute(delegate { TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Authenticate); }));
    }

    private IEnumerator Execute(Action onPlay)
    {
        _tabLoading.Open(10);

        yield return new WaitForSeconds(0.5f);

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
