using UnityEngine.EventSystems;

public class SelectTankButton : BaseSelectTankButton
{
    private SelectTankButton[] _selectTankButtons;


    protected override void Awake()
    {
        base.Awake();
        _selectTankButtons = FindObjectsOfType<SelectTankButton>();
    }

    protected override void Start()
    {
        base.Start();
        SimulateButtonClick();
    }

    private void SimulateButtonClick()
    {
        if (_index == _data.SelectedTankIndex)
        {
            _button.OnSelect(new PointerEventData(EventSystem.current));
            print("Player selected tank button is auto highlighted at the game start/ " + _data.SelectedTankIndex);
        }
    }

    private void OnDeselect()
    {
        _button.OnDeselect(new PointerEventData(EventSystem.current));
    }

    public override void OnClickTankButton()
    {
        if (IsIndexCorrect())
        {
            GlobalFunctions.Loop<SelectTankButton>.Foreach(_selectTankButtons, button => 
            {
                if (button != this) button.OnDeselect();
            });
            _data.SetData(new Data.NewData { SelectedTankIndex = _data.AvailableTanks[_index]._tankIndex });
        }      
    }
}
