using System;

public class Tab_Tanks : BaseTab_Tanks, ITab_Base
{
    private ITab_Base _previousTab;

    public event Action<ITab_Base> onSendTab;


    protected override void OnEnable()
    {
        base.OnEnable();
        MenuTabs.Tab_StartGame.onPlayOffline += OpenTab;
        MenuTabs.Tab_SelectLobby.onSendTab += CachePreviousTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MenuTabs.Tab_StartGame.onPlayOffline -= OpenTab;
        MenuTabs.Tab_SelectLobby.onSendTab -= CachePreviousTab;
    }

    private void CachePreviousTab(ITab_Base previousTab)
    {
        _previousTab = previousTab;
        base.OpenTab();
    }
    protected override void GoForward() => onSendTab?.Invoke(_previousTab);
}
