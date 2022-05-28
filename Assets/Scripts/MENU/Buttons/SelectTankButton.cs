
public class SelectTankButton : BaseSelectTankButton
{
    private Tab_SelectedTanks _tab_SelectedTanks;
    private TanksInfo _tanksInfo;


    protected override void Awake()
    {
        base.Awake();
        _tab_SelectedTanks = Get<Tab_SelectedTanks>.From(gameObject);
        _tanksInfo = Get<TanksInfo>.From(gameObject);
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

    protected override void DisplayTankInfo()
    {
        _tanksInfo.Display(
            new TanksInfo.Info(
                (int)_data.AvailableTanks[_index]._normalSpeed,
                (int)_data.AvailableTanks[_index]._maxForce,
                _data.AvailableTanks[_index]._armor,
                _data.AvailableTanks[_index]._getItNowPrice,
                Converter.HhMMSS(_data.AvailableTanks[_index]._initialBuildHours, _data.AvailableTanks[_index]._initialBuildMinutes, _data.AvailableTanks[_index]._initialBuildSeconds)));
    }

    protected override void InitializeTankStats()
    {
        if (IsIndexCorrect())
        {
            _textTankName.text = _data.AvailableTanks[_index]._tankName;
            _textPlayerLevel.text = "Lvl " + _data.AvailableTanks[_index]._availableInLevel;
            _stars.Display(_data.AvailableTanks[_index]._starsCount);           
        }
    }
}
