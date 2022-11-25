using System;

public class Tab_Tanks : BaseTab_Tanks<Tab_StartGame>, ITab_Base
{
    private ITab_Base _previousTab;

    public event Action<ITab_Base> onSendTab;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _object.onPlayOffline += OpenTab;
        MenuTabs.Tab_SelectLobby.onSendTab += CachePreviousTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _object.onPlayOffline -= OpenTab;
        MenuTabs.Tab_SelectLobby.onSendTab -= CachePreviousTab;
    }

    private void CachePreviousTab(ITab_Base previousTab)
    {
        _previousTab = previousTab;
        base.OpenTab();
    }
    protected override void GoBack() => onSendTab?.Invoke(_previousTab);
}
