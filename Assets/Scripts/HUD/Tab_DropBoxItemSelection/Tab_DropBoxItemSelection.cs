using UnityEngine;
using System;

public class Tab_DropBoxItemSelection : MonoBehaviour, IHudTabsObserver, IEndGame
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private DropBoxItemSelectionPanel _dropBoxItemsSelectionPanel;

    public event Action<bool> onDropBoxItemSelectionTabActivity;

    

    public void SetActivity(bool isActive)
    {
        if (!_canvasGroup.interactable && GameSceneObjectsReferences.BaseRemoteControlTarget.IsActive)
            return;

        GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabDropBoxItemSelection, isActive);
    }

    public void Execute(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        onDropBoxItemSelectionTabActivity?.Invoke(isActive);
    }

    public void OnGameEnd(object[] data = null)
    {
        SetActivity(false);
    }

    public void WrapUpGame(object[] data = null)
    {
        
    }
}
