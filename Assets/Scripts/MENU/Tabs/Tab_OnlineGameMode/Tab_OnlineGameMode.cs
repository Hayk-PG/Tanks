using UnityEngine;

public class Tab_OnlineGameMode : Tab_Base<Tab_SelectedTanks>
{
    private void OnEnable()
    {
        _object.onOnlineModeTankSelected += TankSelected;
    }

    private void OnDisable()
    {
        _object.onOnlineModeTankSelected -= TankSelected;
    }

    private void TankSelected()
    {
        base.OpenTab();
    }
}
