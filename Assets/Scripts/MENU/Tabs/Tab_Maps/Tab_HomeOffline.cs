using System;
using UnityEngine;

public class Tab_HomeOffline : Tab_Base, ITab_Base, IReset
{
    [SerializeField] private Btn _btnTank, _btnAITank;

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
        MenuTabs.Tab_StartGame.onPlayOffline += OpenTab;
        MenuTabs.Tab_Tanks.onSendTab += OpenTab;
        MenuTabs.Tab_AITanks.onSendTab += OpenTab;
        _btnTank.onSelect += delegate { onOpenTabTanks?.Invoke(_iTabBase); };
        _btnAITank.onSelect += delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MenuTabs.Tab_StartGame.onPlayOffline -= OpenTab;
        MenuTabs.Tab_Tanks.onSendTab -= OpenTab;
        MenuTabs.Tab_AITanks.onSendTab -= OpenTab;
        _btnTank.onSelect -= delegate { onOpenTabTanks?.Invoke(_iTabBase); };
        _btnAITank.onSelect -= delegate { onOpenTabAITanks?.Invoke(_iTabBase); };
    }

    private void OpenTab(ITab_Base iTabBase)
    {
        if (_iTabBase == iTabBase)
            base.OpenTab();
    }

    protected override void GoForward()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }

    public void SetDefault()
    {
        _btnTank.Deselect();
        _btnAITank.Deselect();
    }
}
