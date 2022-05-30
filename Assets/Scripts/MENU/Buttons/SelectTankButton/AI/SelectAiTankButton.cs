using System;
using System.Collections.Generic;
using PlayFab.ServerModels;

public class SelectAiTankButton : BaseSelectTankButton
{
    private Tab_SelectedAITanks _tab_SelectedAITanks;


    protected override void Awake()
    {
        base.Awake();
        _tab_SelectedAITanks = Get<Tab_SelectedAITanks>.From(gameObject);
        IsLocked = false;
    }

    private void OnEnable()
    {
        _tab_SelectedAITanks.OnTabOpened += SimulateButtonClick;
    }

    private void OnDisable()
    {
        _tab_SelectedAITanks.OnTabOpened -= SimulateButtonClick;
    }

    protected override bool IsIndexCorrect()
    {
        return _index < _data.AvailableAITanks.Length;
    }

    protected override bool CanAutoClick()
    {
        return _index == _data.SelectedAITankIndex;
    }

    public override void OnClickTankButton()
    {
        if(IsIndexCorrect())
        {
            DeselectAllButtonsAndSelectThis();
            ClickedIndicator();
            _baseTankButtonData.Save(this);
            _baseTankButtonInfo.TankOwnedScreen(this);
        }
    }

    protected override void DeselectAllButtonsAndSelectThis()
    {
        GlobalFunctions.Loop<BaseSelectTankButton>.Foreach(SelectTankButtons, tankButton => 
        {
            if (tankButton == this) ButtonSprite(true, false);
            if (tankButton != this) tankButton.ButtonSprite(false, false);
        });
    }
}
