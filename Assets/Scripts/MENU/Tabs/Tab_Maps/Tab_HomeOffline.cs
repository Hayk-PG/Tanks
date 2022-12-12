using System;
using UnityEngine;

public class Tab_HomeOffline : Tab_Base, ITab_Base, IReset
{
    [SerializeField] private Btn _btnTank, _btnAITank;

    private ITab_Base _iTabBase;
    private MyPhotonCallbacks _myPhotonCallbacks;

    public event Action<ITab_Base> onSendTab;
    public event Action<ITab_Base> onOpenTabTanks;
    public event Action<ITab_Base> onOpenTabAITanks;


    protected override void Awake()
    {
        base.Awake();
        _iTabBase = Get<ITab_Base>.From(gameObject);
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _myPhotonCallbacks._OnConnectedToMaster += OpenTab;
        MenuTabs.Tab_Initialize.onJumpTabOffline += OpenTab;
        MenuTabs.Tab_Tanks.onSendTab += OpenTab;
        MenuTabs.Tab_AITanks.onSendTab += OpenTab;
        _btnTank.onSelect += delegate { onOpenTabTanks?.Invoke(_iTabBase); };
        _btnAITank.onSelect += delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _myPhotonCallbacks._OnConnectedToMaster -= OpenTab;
        MenuTabs.Tab_Initialize.onJumpTabOffline -= OpenTab;
        MenuTabs.Tab_Tanks.onSendTab -= OpenTab;
        MenuTabs.Tab_AITanks.onSendTab -= OpenTab;
        _btnTank.onSelect -= delegate { onOpenTabTanks?.Invoke(_iTabBase); };
        _btnAITank.onSelect -= delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    public override void OpenTab()
    {
        if (!MyPhotonNetwork.IsOfflineMode)
            return;

        base.OpenTab();
        MyPhoton.GameModeRegistered = MyPhoton.RegisteredGameMode.Offline;
    }

    private void OpenTab(ITab_Base iTabBase)
    {
        if (_iTabBase == iTabBase)
            OpenTab();
    }

    protected override void GoForward()
    {
        OpenLoadingTab();
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }

    public void SetDefault()
    {
        _btnTank.Deselect();
        _btnAITank.Deselect();
    }
}
