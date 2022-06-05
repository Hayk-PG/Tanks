
using System;

public class Tab_SelectedAITanks : Tab_Base<Tab_SelectedTanks>
{
    public Action OnAITankSelected { get; set; }

    private void OnEnable()
    {
        _object.OnSingleGameTankSelected += OpenTab;
    }

    private void OnDisable()
    {
        _object.OnSingleGameTankSelected -= OpenTab; 
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }

    public void OnClickSelectButton()
    {
        OnAITankSelected?.Invoke();
    }
}
