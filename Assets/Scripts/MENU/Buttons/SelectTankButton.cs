
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
        _tab_SelectedTanks.OnTabOpened += SimulateButtonClick;
    }

    private void OnDisable()
    {
        _tab_SelectedTanks.OnTabOpened -= SimulateButtonClick;
    }

    protected override bool IsIndexCorrect()
    {
        return _index < _data.AvailableTanks.Length;
    }

    protected override bool CanAutoClick()
    {
        return _index == _data.SelectedTankIndex;
    }

    protected override void SetData()
    {
        _data.SetData(new Data.NewData { SelectedTankIndex = _data.AvailableTanks[_index]._tankIndex });
    }
}
