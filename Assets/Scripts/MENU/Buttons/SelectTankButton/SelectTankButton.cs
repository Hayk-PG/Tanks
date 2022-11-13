using System;
using System.Collections.Generic;
using PlayFab.ServerModels;

public class SelectTankButton : BaseSelectTankButton
{
    private Tab_SelectedTanks _tab_SelectedTanks;


    protected override void Awake()
    {
        base.Awake();       
        _tab_SelectedTanks = Get<Tab_SelectedTanks>.From(gameObject);
    }

    private void OnEnable()
    {
        _tab_SelectedTanks.onTabOpen += OnTabSelectedTanksOpen;
    }

    private void OnDisable()
    {
        _tab_SelectedTanks.onTabOpen -= OnTabSelectedTanksOpen;
    }

    protected override bool IsIndexCorrect()
    {
        return _index < _data.AvailableTanks.Length;
    }

    protected override bool CanAutoClick()
    {
        return _index == _data.SelectedTankIndex;
    }

    private void OnTabSelectedTanksOpen()
    {
        if (IsIndexCorrect())
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, OfflineMode, GetUserReadOnlyData);
        }
    }

    private void OfflineMode()
    {
        ButtonSprite(false, false);
        IsLocked = false;
        SimulateButtonClick();
    }

    private void GetUserReadOnlyData()
    {
        ButtonSprite(false, true);
        IsClicked = false;
        IsLocked = true;

        MyPlayfab.Manager.GetUserReadOnlyData(Data.Manager.PlayfabId, readonlyData =>
        {
            if (readonlyData.ContainsKey(_data.AvailableTanks[_index]._tankName))
            {
                IsLocked = false;
                ButtonSprite(false, false);
                SimulateButtonClick();


                //bool isCurrentTankButtonLocked = readonlyData[_data.AvailableTanks[_index]._tankName].Value == PlayfabKeyAndValues.Locked;
                //bool isCurrentTankButtonUnlocked = readonlyData[_data.AvailableTanks[_index]._tankName].Value == PlayfabKeyAndValues.UnLocked;
                //Conditions<bool>.Compare(isCurrentTankButtonLocked, isCurrentTankButtonUnlocked, OnLocked, null, OnUnlocked, null);
            }
        });
    }
}
