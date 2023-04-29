using UnityEngine;
using System;

public class Tab_DropBoxItemSelection : MonoBehaviour, IHudTabsObserver, IEndGame
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private DropBoxItemSelectionPanel _dropBoxItemsSelectionPanel;

    [SerializeField] [Space]
    private CustomScrollRect _customScrollRect;

    [SerializeField] [Space]
    private Btn _btnClose;

    public event Action<bool> onDropBoxItemSelectionTabActivity;





    private void OnEnable() => _btnClose.onSelect += () => SetActivity(false);

    public void SetActivity(bool isActive)
    {
        if (!_canvasGroup.interactable && GameSceneObjectsReferences.BaseRemoteControlTarget.IsActive)
            return;

        GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabDropBoxItemSelection, isActive);
    }

    public void Execute(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        ResetCustomScrollRect(isActive);

        PlaySoundFX(isActive);

        onDropBoxItemSelectionTabActivity?.Invoke(isActive);
    }

    private void ResetCustomScrollRect(bool isActive)
    {
        if (!isActive)
            return;

        _customScrollRect.SetDefault();
    }

    private void PlaySoundFX(bool isActive)
    {
        if (isActive)
            UISoundController.PlaySound(1, 0);
    }

    public void OnGameEnd(object[] data = null)
    {
        SetActivity(false);
    }

    public void WrapUpGame(object[] data = null)
    {
        
    }
}
