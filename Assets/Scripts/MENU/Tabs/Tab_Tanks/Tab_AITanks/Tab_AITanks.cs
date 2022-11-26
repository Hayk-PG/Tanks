using System;

public class Tab_AITanks : BaseTab_Tanks, ITab_Base
{
    public event Action<ITab_Base> onSendTab;


    protected override void OnEnable()
    {
        base.OnEnable();
        MenuTabs.Tab_HomeOffline.onOpenTabAITanks += CachePreviousTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MenuTabs.Tab_HomeOffline.onOpenTabAITanks -= CachePreviousTab;
    }

    protected override void GoForward() => onSendTab?.Invoke(_previousTab);
}
