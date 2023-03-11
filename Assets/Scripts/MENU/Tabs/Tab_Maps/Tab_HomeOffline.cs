using System;
using UnityEngine;
using TMPro;

public class Tab_HomeOffline : Tab_Base, ITab_Base, IReset
{
    [SerializeField] 
    private Btn _btnTank, _btnAITank;

    [SerializeField] [Space]
    private TMP_Text _txtMapName;

    private ITab_Base _iTabBase;

    public event Action<ITab_Base> onSendTab;
    public event Action<ITab_Base> onOpenTabTanks;
    public event Action<ITab_Base> onOpenTabAITanks;


    protected override void Awake()
    {
        base.Awake();

        _iTabBase = Get<ITab_Base>.From(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_Tanks.onSendTab += OpenTab;

        MenuTabs.Tab_AITanks.onSendTab += OpenTab;

        _btnTank.onSelect += delegate { onOpenTabTanks?.Invoke(_iTabBase); };

        _btnAITank.onSelect += delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        MenuTabs.Tab_Tanks.onSendTab -= OpenTab;

        MenuTabs.Tab_AITanks.onSendTab -= OpenTab;

        _btnTank.onSelect -= delegate { onOpenTabTanks?.Invoke(_iTabBase); };

        _btnAITank.onSelect -= delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    protected override void OnOperationSubmitted(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if(operation == TabsOperation.Operation.PlayOffline)
        {
            OperationHandler = handler;

            OpenTab();
        }
    }

    public override void OpenTab()
    {
        MyPhoton.GameModeRegistered = MyPhoton.RegisteredGameMode.Offline;

        MyPhotonNetwork.ManageOfflineMode(true);

        OperationHandler.OnOperationSucceded();

        base.OpenTab();
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

    public void DisplayMapName(Map map)
    {
        _txtMapName.text = map.MapName;
    }

    public void SetDefault()
    {
        _btnTank.Deselect();

        _btnAITank.Deselect();
    }
}
