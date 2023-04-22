using UnityEngine;
using System;

public class HudTabsHandler : MonoBehaviour
{
    public enum HudTab { None, GameplayAnnouncer, AmmoTypeController, TabRemoteControl, TabRocketController, TabModify, TabDropBoxItemSelection}

    private HudTab _currentActiveTab;

    public event Action<IHudTabsObserver, HudTab, HudTab, bool> onRequestTabActivityPermission;




    public void RequestTabActivityPermission(IHudTabsObserver observer, HudTab requestedTab, bool isActive)
    {
        onRequestTabActivityPermission?.Invoke(observer, _currentActiveTab, requestedTab, isActive);
    }

    public void SetCurrentActiveTab(HudTab currentActiveTab)
    {
        _currentActiveTab = currentActiveTab;
    }
}
