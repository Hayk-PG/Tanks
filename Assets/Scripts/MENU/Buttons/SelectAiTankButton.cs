
public class SelectAiTankButton : BaseSelectTankButton
{
    private Tab_SelectedAITanks _tab_SelectedAITanks;


    protected override void Awake()
    {
        base.Awake();
        _tab_SelectedAITanks = Get<Tab_SelectedAITanks>.From(gameObject);
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

    protected override void SetData()
    {
        _data.SelectedAITankIndex = _data.AvailableAITanks[_index]._tankIndex;
    }

    protected override void DisplayTankInfo()
    {
        
    }

    protected override void InitializeTankStats()
    {
        
    }
}
