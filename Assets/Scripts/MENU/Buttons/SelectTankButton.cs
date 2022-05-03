using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectTankButton : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private Image _iconTank;
    private Button _button;
    private Data _data;

    private SelectTankButton[] _selectTankButtons;

    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
        _data = FindObjectOfType<Data>();
        _selectTankButtons = FindObjectsOfType<SelectTankButton>();
    }

    private void Start()
    {
        InitializeButton();
        SimulateButtonClick();
    }

    private bool IsIndexCorrect()
    {
        return _index < _data.AvailableTanks.Length;
    }

    private void InitializeButton()
    {
        if (IsIndexCorrect()) _iconTank.sprite = _data.AvailableTanks[_index]._iconTank;
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

    public void OnClickTankButton()
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
