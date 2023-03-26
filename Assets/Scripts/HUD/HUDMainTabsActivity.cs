using UnityEngine;
using System.Collections.Generic;
using System;

public class HUDMainTabsActivity : MonoBehaviour, IEndGame
{
    [SerializeField] [Space]
    private UpperTab _upperTab;

    [SerializeField] [Space]
    private AmmoTab _ammoTab;

    [SerializeField] [Space]
    private HudMainTab _hudMainTab;

    private HudTabsHandler.HudTab _currentActiveTab;

    private IHudTabsObserver _currentObserver;

    private Action _queuedExecution;

    private PlayerTurn LocalPlayerTurn { get; set; }

    private bool IsGameStartAnnounced { get; set; }
    private bool IsMyTurn { get; set; }
    private bool IsGameEnd { get; set; }
    



    private void Awake() => SetAllMainTabsActivities(false, false, false);

    private void OnEnable()
    {
        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChange;

        GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission += OnRequestTabActivityPermission;
    }

    private void OnTurnChange(TurnState turnState)
    {
        if (LocalPlayerTurn == null)
            LocalPlayerTurn = GlobalFunctions.ObjectsOfType<TankController>.Find(tk => tk.BasePlayer != null)?.GetComponent<PlayerTurn>();

        IsMyTurn = LocalPlayerTurn?.MyTurn == turnState;

        if (!IsGameStartAnnounced)
            return;

        SetAllMainTabsActivities(true, true, true);

        _queuedExecution?.Invoke();
    }

    private void OnRequestTabActivityPermission(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        if(isActive && IsGameEnd)
        {
            SetAllMainTabsActivities();

            return;
        }

        switch (requestedTab)
        {
            case HudTabsHandler.HudTab.GameplayAnnouncer: OnGameplayAnnouncer(observer, currentActiveTab, requestedTab, isActive); break;

            case HudTabsHandler.HudTab.AmmoTypeController: OnAmmoTypeController(observer, currentActiveTab, requestedTab, isActive); break;

            case HudTabsHandler.HudTab.TabRemoteControl: OnTabRemoteControl(observer, currentActiveTab, requestedTab, isActive); break;

            case HudTabsHandler.HudTab.TabRocketController: OnTabRocketController(observer, currentActiveTab, requestedTab, isActive); break;

            case HudTabsHandler.HudTab.TabModify: OnTabModify(observer, currentActiveTab, requestedTab, isActive); break;

            case HudTabsHandler.HudTab.TabDropBoxItemSelection: OnTabDropBoxItemSelection(observer, currentActiveTab, requestedTab, isActive); break;
        }
    }

    #region OnGameplayAnnouncer
    private void OnGameplayAnnouncer(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        IsGameStartAnnounced = true;

        SetAllMainTabsActivities(true, true, true);
    }
    #endregion

    #region OnAmmoTypeController
    private void OnAmmoTypeController(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        observer?.Execute(isActive);

        SetCurrentObserver(isActive, observer);

        SetCurrentTab(isActive, requestedTab);

        SetAllMainTabsActivities(!isActive, !isActive);
    }
    #endregion

    #region OnTabRemoteControl
    private void OnTabRemoteControl(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        observer.Execute(isActive);

        SetCurrentObserver(isActive, observer);

        SetCurrentTab(isActive, requestedTab);

        SetAllMainTabsActivities(!isActive, !isActive, !isActive);
    }
    #endregion

    #region OnTabRocketController
    private void OnTabRocketController(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        observer.Execute(isActive);

        SetCurrentObserver(isActive, observer);

        SetCurrentTab(isActive, requestedTab);

        SetAllMainTabsActivities(!isActive, !isActive, !isActive);
    }
    #endregion

    #region OnTabModify
    private void OnTabModify(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        observer.Execute(isActive);

        SetCurrentObserver(isActive, observer);

        SetCurrentTab(isActive, requestedTab);

        SetAllMainTabsActivities(!isActive, !isActive, !isActive);
    }
    #endregion

    #region OnTabDropBoxItemSelection
    private void OnTabDropBoxItemSelection(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        if (isActive)
        {
            bool queue = !IsMyTurn || currentActiveTab == HudTabsHandler.HudTab.TabRemoteControl || currentActiveTab == HudTabsHandler.HudTab.TabRocketController;

            if (queue)
            {
                _queuedExecution = ()=> Execute(observer, requestedTab, true, isActive);

                return;
            }

            CloseDropBoxItemSelecionTab(currentActiveTab);
        }

        bool setCurrentActiveTab = !isActive && currentActiveTab != HudTabsHandler.HudTab.TabRemoteControl ||
                                   !isActive && currentActiveTab != HudTabsHandler.HudTab.TabRocketController;

        Execute(observer, requestedTab, setCurrentActiveTab, isActive);
    }

    // Close if certain tabs are open
    private void CloseDropBoxItemSelecionTab(HudTabsHandler.HudTab currentActiveTab)
    {
        bool closeCurrentTab = currentActiveTab != HudTabsHandler.HudTab.None || currentActiveTab != HudTabsHandler.HudTab.AmmoTypeController ||
                               currentActiveTab != HudTabsHandler.HudTab.GameplayAnnouncer || currentActiveTab != HudTabsHandler.HudTab.TabDropBoxItemSelection;

        if (closeCurrentTab)
            _currentObserver?.Execute(false);
    }

    private void Execute(IHudTabsObserver observer, HudTabsHandler.HudTab requestedTab, bool setCurrentActiveTab, bool isActive)
    {
        observer.Execute(isActive);

        SetCurrentObserver(isActive, observer);

        TrySetCurrentActiveTab(setCurrentActiveTab, isActive, observer, requestedTab);

        SetAllMainTabsActivities(!isActive, !isActive, !isActive);

        _queuedExecution = null;
    }

    private void TrySetCurrentActiveTab(bool setCurrentActiveTab, bool isActive, IHudTabsObserver observer, HudTabsHandler.HudTab requestedTab)
    {
        if (!setCurrentActiveTab)
            SetCurrentTab(isActive, requestedTab);
    }
    #endregion

    private void SetCurrentObserver(bool isActive, IHudTabsObserver observer)
    {
        if (!isActive)
        {
            _currentObserver = null;

            return;
        }

        _currentObserver = observer;
    }

    private void SetCurrentTab(bool isActive, HudTabsHandler.HudTab hudTab)
    {
        if (!isActive)
        {
            _currentActiveTab = HudTabsHandler.HudTab.None;

            GameSceneObjectsReferences.HudTabsHandler.SetCurrentActiveTab(_currentActiveTab);

            return;
        }

        _currentActiveTab = hudTab;

        GameSceneObjectsReferences.HudTabsHandler.SetCurrentActiveTab(_currentActiveTab);
    }

    private void SetAllMainTabsActivities(bool? isUpperTabActive = null, bool? isMainTabActive = null, bool? isAmmoTabActive = null)
    {
        bool closeAllTabs = IsGameEnd || _currentActiveTab == HudTabsHandler.HudTab.TabDropBoxItemSelection ||
                            _currentActiveTab == HudTabsHandler.HudTab.TabModify ||
                            _currentActiveTab == HudTabsHandler.HudTab.TabRemoteControl ||
                            _currentActiveTab == HudTabsHandler.HudTab.TabRocketController;

        if (closeAllTabs)
        {
            _upperTab.Execute(false);

            _hudMainTab.Execute(false);

            _ammoTab.Execute(false);

            return;
        }

        bool isAmmoTypeControllerActive = _currentActiveTab == HudTabsHandler.HudTab.AmmoTypeController;

        if (isAmmoTypeControllerActive)
        {
            _upperTab.Execute(false);

            _hudMainTab.Execute(false);

            return;
        }

        if (IsMyTurn)
        {
            if (isMainTabActive.HasValue)
                _hudMainTab.Execute(isMainTabActive.Value);
        }
        else
        {
            _hudMainTab.Execute(false);
        }

        if (isUpperTabActive.HasValue)
            _upperTab.Execute(isUpperTabActive.Value);

        if (isAmmoTabActive.HasValue)
            _ammoTab.Execute(isAmmoTabActive.Value);
    }

    public void OnGameEnd(object[] data = null) => IsGameEnd = true;

    public void WrapUpGame(object[] data = null)
    {
        
    }
}
